using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;

namespace PuroBot.Discord.Modules
{
    [Group("voice")]
    [Summary("voice chat-related commands")]
    public class VoiceConnectionModule : ModuleBase<SocketCommandContext>
    {
        public VoiceConnectionModule(VoiceConnectionService voice)
        {
            _voice = voice;
        }

        [Command("join")]
        [Summary("join the current voice channel")]
        [RequireContext(ContextType.Guild)]
        public async Task JoinCommand(IVoiceChannel? channel = null)
        {
            // join, but release the lock immediately after, as to not block other tasks from playing audio
            // this also means the connection timeout will start immediately
            using var channelAcquirer = await VoiceConnectionService.ChannelHandle.Create(_voice, Context, channel);
        }

        [Command("leave")]
        [Summary("leave the voice channel")]
        [RequireContext(ContextType.Guild)]
        public async Task LeaveCommand()
        {
            await _voice.LeaveChannel(Context);
        }

        private readonly VoiceConnectionService _voice;
    }
}
