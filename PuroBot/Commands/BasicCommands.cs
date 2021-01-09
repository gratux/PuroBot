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
	}
}