using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace PuroBot.Commands
{
	[Group("sound")]
	[Description("voice chat-related commands")]
	public class SoundCommands : BaseCommandModule
	{
		[Command("join")]
		[Description("join the current voice channel")]
		public async Task JoinCommand(CommandContext ctx, DiscordChannel channel = null)
		{
			Program.SoundTimeoutManager.AddOrUpdate(ctx.Guild);

			channel ??= ctx.Member.VoiceState?.Channel;

			var vnext = ctx.Client.GetVoiceNext();
			var connection = vnext?.GetConnection(ctx.Guild);
			if (connection?.TargetChannel == channel)
				return; // already in correct channel

			connection?.Disconnect();
			await channel.ConnectAsync();
		}

		[Command("leave")]
		[Description("leave the voice channel")]
		public async Task LeaveCommand(CommandContext ctx)
		{
			Program.SoundTimeoutManager.Remove(ctx.Guild);

			var vnext = ctx.Client.GetVoiceNext();
			var connection = vnext?.GetConnection(ctx.Guild);

			connection?.Disconnect();
		}
	}
}