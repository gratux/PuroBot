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
			
			var useragent = $"PuroBot/{Assembly.GetExecutingAssembly().GetName().Version} (by d3r_5h06un)";

			var e621 = new E621Client(useragent);
			var searchTask = e621.Search(new E621SearchOptions()
			{
				Tags = $"puro_(changed) solo rating:s order:random score:>={minScore}",
				Limit = 1
			});
			
			try
			{
				await searchTask;
				var posts = searchTask.Result;
				await ctx.RespondAsync(posts[0].File.Url);
			}
			catch
			{
				await ctx.RespondAsync("My camera is broken... Sorry Human.");
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