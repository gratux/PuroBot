using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace PuroBot.Commands
{
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	internal class BasicCommands : BaseCommandModule
	{
		[Command("ping")]
		[Description("The human will be greeted")]
		private async Task PingCommand(CommandContext ctx)
		{
			await ctx.RespondAsync($"Hello {ctx.Member.Mention}!");
		}
	}
}