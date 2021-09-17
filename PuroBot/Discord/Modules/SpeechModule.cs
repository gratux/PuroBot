using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;
using PuroBot.Extensions;

namespace PuroBot.Discord.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireContext(ContextType.Guild)]
    public class SpeechModule : ModuleBase<SocketCommandContext>
    {
        public SpeechModule(VoiceConnectionService voice, SpeechService speech)
        {
            _voice = voice;
            _speech = speech;
        }

        [Command("espeak")]
        public async Task ESpeakCommand([Remainder] string message)
        {
            string tempWav = Path.GetTempFileName();

            var procInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/espeak-ng", ArgumentList = { "-w", tempWav, message }, UseShellExecute = false
            };
            var proc = new Process { StartInfo = procInfo };
            proc.Start();
            await proc.WaitForExitAsync();

            procInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/ffmpeg",
                ArgumentList =
                {
                    "-i",
                    tempWav,
                    "-ac",
                    "2",
                    "-ar",
                    "48000",
                    "-f",
                    "s16le",
                    "-"
                },
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            proc = new Process { StartInfo = procInfo };
            proc.Start();

            using var channelAcquirer = await VoiceConnectionService.ChannelHandle.Create(_voice, Context);
            VoiceConnectionService.VoiceInfo? voiceInfo = channelAcquirer.VoiceInfo;
            if (voiceInfo is null) //failed to connect
                return;

            await proc.StandardOutput.BaseStream.CopyToAsync(voiceInfo.AudioStream);
        }

        [Command("speak")]
        [Alias("tts")]
        public async Task SpeechCommand([Remainder] string message)
        {
            using var channelAcquirer = await VoiceConnectionService.ChannelHandle.Create(_voice, Context);
            VoiceConnectionService.VoiceInfo? voiceInfo = channelAcquirer.VoiceInfo;
            if (voiceInfo is null) //failed to connect
                return;

            byte[] pcm = _speech.SynthesizeMessageAsync(message).NormalizeAudio().ToArray();
            await voiceInfo.AudioStream.WriteAsync(pcm);
        }

        private readonly SpeechService _speech;
        private readonly VoiceConnectionService _voice;
    }
}
