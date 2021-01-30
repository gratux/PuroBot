using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Services;

namespace PuroBot.Modules
{
	[Group("sound")]
	[Summary("voice chat-related commands")]
	public class VoiceModule : ModuleBase<SocketCommandContext>
	{
		private readonly VoiceService _voice;

		public VoiceModule(VoiceService voice)
		{
			_voice = voice;
		}

		[Command("join")]
		[Summary("join the current voice channel")]
		public async Task JoinCommand(IVoiceChannel channel = null)
		{
			await _voice.JoinChannel(Context, channel);
		}

		[Command("leave")]
		[Summary("leave the voice channel")]
		public async Task LeaveCommand()
		{
			await _voice.LeaveChannel(Context);
		}
	}
}