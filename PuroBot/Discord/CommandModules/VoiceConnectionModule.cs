using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;

namespace PuroBot.Discord.CommandModules
{
	[Group("voice")]
	[Summary("voice chat-related commands")]
	public class VoiceConnectionModule : ModuleBase<SocketCommandContext>
	{
		private readonly VoiceConnectionService _voice;

		public VoiceConnectionModule(VoiceConnectionService voice) => _voice = voice;

		[Command("join")]
		[Summary("join the current voice channel")]
		[RequireContext(ContextType.Guild)]
		public async Task JoinCommand(IVoiceChannel channel = null)
		{
			// join, but release the lock immediately after, as to not block other tasks from playing audio
			// this also means the connection timeout will start immediately
			await _voice.AcquireChannel(Context, channel);
			_voice.ReleaseChannel(Context);
		}

		[Command("leave")]
		[Summary("leave the voice channel")]
		[RequireContext(ContextType.Guild)]
		public async Task LeaveCommand()
		{
			_voice.ReleaseChannel(Context);
			await _voice.LeaveChannel(Context);
		}
	}
}