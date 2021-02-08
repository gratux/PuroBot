using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using PuroBot.Extensions;
using SodiumDL;

namespace PuroBot.CommandModules
{
	public class ImageModule : ModuleBase<SocketCommandContext>
	{
		private static readonly string UserAgent =
			$"PuroBot/{Assembly.GetExecutingAssembly().GetName().Version} (by d3r_5h06un)";

		[Command("e621")]
		[Description("get posts from e621.net")]
		[RequireNsfw]
		public async Task E621Command(
			[Description("the number of posts to request (default: 5)")]
			int count = 5,
			[Remainder] [Description("the tags to be searched (default: none)")]
			string tags = "")
		{
			var e621 = new SodiumClient(userAgent: UserAgent);
			var posts = await e621.GetPostsAsync(tags, count);
			await ReplyAsync("Here you go, human:");
			await posts.Select(p => p.File?.AbsoluteUri).SendMany(msg => ReplyAsync(msg));
		}

		[Command("selfie")]
		[Alias("s")]
		[Description("Get a random image of me")]
		[RequireNsfw]
		public async Task SelfieCommand([Description("minimum e621 score (default: 10)")]
			int minScore = 10)
		{
			var e621 = new SodiumClient(userAgent: UserAgent);
			var posts = await e621.GetPostsAsync($"puro_(changed) rating:s solo score:>{minScore}", 1);
			var url = posts.FirstOrDefault()?.File.AbsoluteUri;
			if (string.IsNullOrWhiteSpace(url))
				await ReplyAsync("My camera is broken... Sorry Human.");
			else
				await ReplyAsync(url);
		}
	}
}