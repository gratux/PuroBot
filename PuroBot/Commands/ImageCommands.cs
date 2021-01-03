using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using E621;

namespace PuroBot.Commands
{
	public class ImageCommands : BaseCommandModule
	{
		[Command("e621"), Description("get posts from e621.net")]
		public async Task E621Command(CommandContext ctx,
			[Description("the number of posts to request")]
			int count = 5,
			[RemainingText] [Description("the tags to be searched")]
			string tags = "")
		{
			if (!ctx.Channel.IsNSFW)
			{
				await ctx.RespondAsync("Sorry human, i can only do this in NSFW channels...");
				return;
			}

			var e621 = new E621Client();
			var posts = e621.Search(new E621SearchOptions()
			{
				Tags = tags,
				Limit = count
			}).Result.Select(p => p.File.Url);
			await ctx.RespondAsync($"Here you go, human!\n{string.Join('\n', posts)}");
		}

		[Command("selfie"), Aliases("s"), Description("Get a random image of me")]
		async Task SelfieCommand(CommandContext ctx, [Description("minimum e621 score (default: 10)")]
			int minScore = 10)
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
	}
}