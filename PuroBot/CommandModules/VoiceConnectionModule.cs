using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Services;

namespace PuroBot.CommandModules
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
			await _voice.JoinOrReuseChannel(Context, channel);
		}

		[Command("leave")]
		[Summary("leave the voice channel")]
		[RequireContext(ContextType.Guild)]
		public async Task LeaveCommand()
		{
			await _voice.LeaveChannel(Context);
		}
	}
}