using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace PuroBot.Commands
{
	public class BasicCommands : BaseCommandModule
	{
		[Command("ping"),Description("Greet the user")]
		async Task PingCommand(CommandContext ctx)
		{
			await ctx.RespondAsync($"Hello Human {ctx.Member.Mention}!");
		}

		[Command("selfie"), Aliases("s")]
		async Task SelfieCommand(CommandContext ctx)
		{
			//TODO: get random image from e926.net
			await ctx.RespondAsync("My camera is broken... Sorry Human.");
		}

		[Command("uwuify"), Aliases("uwu"), Description("translate a message to uwu-speak")]
		async Task UwuCommand(CommandContext ctx, [RemainingText] string message)
		{
			var translated = message.Replace('l', 'w').Replace('r', 'w');
			await ctx.RespondAsync($"I translated your message, human:\n> {translated}");
		}
	}
}