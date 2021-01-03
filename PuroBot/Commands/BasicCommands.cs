using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using E621;

namespace PuroBot.Commands
{
	public class BasicCommands : BaseCommandModule
	{
		[Command("ping"), Description("The human will be greeted")]
		async Task PingCommand(CommandContext ctx)
		{
			await ctx.RespondAsync($"Hello {ctx.Member.Mention}!");
		}

		[Command("selfie"), Aliases("s"), Description("Get a random image of me")]
		async Task SelfieCommand(CommandContext ctx, [Description("minimum e621 score (default: 10)")] int minScore = 10)
		{
			if (!ctx.Channel.IsNSFW) //e621 rating safe may not be appropriate in SFW channels
			{
				await ctx.RespondAsync("Sorry human, i can only do this in NSFW channels...");
				return;
			}

			var e621 = new E621Client();
			var url = e621.GetRandomPostsUrl("puro_(changed) rating:s solo", minScore, 1).Result.First();
			if (url == null)
			{
				await ctx.RespondAsync("My camera is broken... Sorry Human.");
			}
			else
			{
				await ctx.RespondAsync(url);
			}
		}

		[Command("uwuify"), Aliases("uwu"), Description("translate a message to uwu-speak")]
		async Task UwuCommand(CommandContext ctx, [RemainingText, Description("the message i should translate")] string message)
		{
			var translated = message.Replace('l', 'w').Replace('r', 'w');
			await ctx.RespondAsync($"I translated your message, human:\n> {translated}");
		}
	}
}