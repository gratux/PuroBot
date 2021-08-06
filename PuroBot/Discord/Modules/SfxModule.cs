using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;
using PuroBot.Extensions;

namespace PuroBot.Discord.Modules
{
    [Group("sfx")]
    [Summary("play a sound file")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireContext(ContextType.Guild)]
    public class SfxModule : ModuleBase<SocketCommandContext>
    {
        public SfxModule(VoiceConnectionService voice)
        {
            _voice = voice;
        }

        private const string BaseAudioPath = "Resources/SpeakAudio/";
        private const string AudioExt = "pcm";

        [Command("list")]
        [Summary("list available sound files")]
        [Priority(1)]
        public async Task ListSpeakCommand()
        {
            var files = new DirectoryInfo(BaseAudioPath).GetFileTree($"*.{AudioExt}").Skip(1); // skip name of top directory
            await ReplyAsync($"Available files are:{string.Join('\n', files).AsCode(true)}");
        }

        [Command]
        [Summary("play a sound effect")]
        [Priority(0)]
        public async Task SpeakCommand([Summary("the filename")] string filename)
        {
            string[] files = Directory.GetFiles(BaseAudioPath, $"*.{AudioExt}", SearchOption.AllDirectories);

            string? path = files.FirstOrDefault(f => Path.GetRelativePath(BaseAudioPath, f) == $"{filename}.{AudioExt}");

            if (string.IsNullOrWhiteSpace(path))
            {
                // file not found
                await ReplyAsync("This isn't the file you are looking for");
                return;
            }

            using var channelAcquirer = await VoiceConnectionService.ChannelHandle.Create(_voice, Context);
            VoiceConnectionService.VoiceInfo? voiceInfo = channelAcquirer.VoiceInfo;
            if (voiceInfo is null) //failed to connect
                return;

            byte[] pcm = (await File.ReadAllBytesAsync(path)).NormalizeAudio().ToArray();
            await voiceInfo.AudioStream.WriteAsync(pcm);
        }

        private readonly VoiceConnectionService _voice;
    }
}
