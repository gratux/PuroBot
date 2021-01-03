using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace PuroBot.Commands
{
	public class BasicCommands : BaseCommandModule
	{
		[Command("ping"), Description("The human will be greeted")]
		async Task PingCommand(CommandContext ctx)
		{
			await ctx.RespondAsync($"Hello {ctx.Member.Mention}!");
		}

		[Command("uwuify"), Aliases("uwu"), Description("translate a message to uwu-speak")]
		async Task UwuCommand(CommandContext ctx, [RemainingText, Description("the message i should translate")] string message)
		{
			var translated = message.Replace('l', 'w').Replace('r', 'w');
			await ctx.RespondAsync($"I translated your message, human:\n> {translated}");
		}
	}
}