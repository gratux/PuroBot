using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;

namespace PuroBot.Discord.Modules
{
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        public MusicModule(VoiceConnectionService voice)
        {
            _voice = voice;
        }

        private static CancellationTokenSource _ctsPlaylist = new();
        private static CancellationTokenSource _ctsSingle = new();

        // [Command("play")]
        // [Summary("play music from a link")]
        // public async Task Play([Remainder] [Summary("the link to play")] Url target)
        // {
        //     await PlayInternal(target.Value);
        // }

        [Command("play")]
        [Summary("play music from a youtube search")]
        public async Task Play([Remainder] [Summary("the youtube search")] string query)
        {
            await Stop();
            _ctsPlaylist.Dispose();
            _ctsPlaylist = new CancellationTokenSource();

            string target = Uri.TryCreate(query, UriKind.Absolute, out _) ? query : $"ytsearch:{query}";
            try
            {
                await foreach (var url in GetUrls(target, _ctsPlaylist.Token))
                {
                    _ctsSingle.Dispose();
                    _ctsSingle = new CancellationTokenSource();
                    try
                    {
                        await PlayUrl(url, _ctsSingle.Token);
                    }
                    catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException) { }
                }
            }
            catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException) { }
        }

        [Command("skip")]
        [Summary("skip one playlist entry")]
        public Task Skip()
        {
            _ctsSingle.Cancel(false);
            return Task.CompletedTask;
        }

        [Command("stop")]
        [Summary("stops music playback")]
        public Task Stop()
        {
            _ctsPlaylist.Cancel(false);
            _ctsSingle.Cancel(false);
            return Task.CompletedTask;
        }

        private readonly VoiceConnectionService _voice;

        private async IAsyncEnumerable<string> GetUrls(string query, [EnumeratorCancellation] CancellationToken ct)
        {
            const string ytdlPath = "/usr/bin/youtube-dl";
            const string ytdlpPath = "/usr/bin/youtube-dlp";

            string downloaderPath = File.Exists(ytdlpPath) ? ytdlpPath : ytdlPath;

            var ytdlInfo = new ProcessStartInfo
            {
                FileName = downloaderPath,
                Arguments = $"-gf bestaudio \"{query}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using Process? ytdl = Process.Start(ytdlInfo);

            if (ytdl is null)
            {
                await Context.Message.ReplyAsync("ytdl process could not be started");
                yield break;
            }

            while (!ct.IsCancellationRequested && !ytdl.HasExited)
            {
                string? line = await ytdl.StandardOutput.ReadLineAsync();
                if (line is not null)
                    yield return line;
            }

            await ytdl.WaitForExitAsync(ct);
            if (ytdl.ExitCode <= 0)
                yield break;

            await Context.Message.ReplyAsync(await ytdl.StandardError.ReadToEndAsync());
        }

        private async Task PlayUrl(string target, CancellationToken ct)
        {
            const string ffmpegPath = "/usr/bin/ffmpeg";

            var ffmpegInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i {target} -ac 2 -ar 48000 -f s16le -",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            using Process? ffmpeg = Process.Start(ffmpegInfo);

            if (ffmpeg is null)
            {
                await Context.Message.ReplyAsync("ffmpeg process could not be started");
                return;
            }

            using var channelAcquirer = await VoiceConnectionService.ChannelHandle.Create(_voice, Context);
            VoiceConnectionService.VoiceInfo? voiceInfo = channelAcquirer.VoiceInfo;
            if (voiceInfo is null) //failed to connect
                return;

            await ffmpeg.StandardOutput.BaseStream.CopyToAsync(voiceInfo.AudioStream, ct);
        }
    }
}
