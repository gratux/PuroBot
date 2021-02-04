using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using PuroBot.Extensions;

namespace PuroBot.CommandModules
{
	public class ImageModule : ModuleBase<SocketCommandContext>
	{
		[Command("e621")]
		[Description("get posts from e621.net")]
		[RequireNsfw]
		public async Task E621Command(
			[Description("the number of posts to request (default: 5)")]
			int count = 5,
			[Remainder] [Description("the tags to be searched (default: none)")]
			string tags = "")
		{
			var e621 = new E621Client();
			var posts = e621.GetPostUrls(tags, count).Result;
			await ReplyAsync("Here you go, human:");
			await posts.SendMany(msg => ReplyAsync(msg));
		}

		[Command("selfie")]
		[Alias("s")]
		[Description("Get a random image of me")]
		[RequireNsfw]
		public async Task SelfieCommand([Description("minimum e621 score (default: 10)")]
			int minScore = 10)
		{
			var e621 = new E621Client();
			var url = e621.GetPostUrls("puro_(changed) rating:s solo", 1, minScore).Result.FirstOrDefault();
			if (string.IsNullOrWhiteSpace(url))
				await ReplyAsync("My camera is broken... Sorry Human.");
			else
				await ReplyAsync(url);
		}
	}
}