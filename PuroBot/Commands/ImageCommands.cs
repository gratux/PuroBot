using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace PuroBot.Commands
{
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	internal class ImageCommands : BaseCommandModule
	{
		[Command("e621")]
		[Description("get posts from e621.net")]
		private async Task E621Command(CommandContext ctx,
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
			var posts = e621.GetPostUrls(tags, count).Result;
			await ctx.RespondAsync("Here you go, human:");
			await Helpers.SendMany(posts, msg => ctx.RespondAsync(msg));
		}

		[Command("selfie")]
		[Aliases("s")]
		[Description("Get a random image of me")]
		private async Task SelfieCommand(CommandContext ctx, [Description("minimum e621 score (default: 10)")]
			int minScore = 10)
		{
			if (!ctx.Channel.IsNSFW) //e621 rating safe may not be appropriate in SFW channels
			{
				await ctx.RespondAsync("Sorry human, i can only do this in NSFW channels...");
				return;
			}

			var e621 = new E621Client();
			var url = e621.GetPostUrls("puro_(changed) rating:s solo", 1, minScore).Result.FirstOrDefault();
			if (string.IsNullOrWhiteSpace(url))
				await ctx.RespondAsync("My camera is broken... Sorry Human.");
			else
				await ctx.RespondAsync(url);
		}
	}
}