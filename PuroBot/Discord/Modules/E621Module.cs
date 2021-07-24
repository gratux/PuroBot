using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using PuroBot.Extensions;
using SodiumDL;

namespace PuroBot.Discord.Modules
{
	public class E621Module : ModuleBase<SocketCommandContext>
	{
		private static readonly string UserAgent =
			$"PuroBot/{Assembly.GetExecutingAssembly().GetName().Version} (by d3r_5h06un)";

		[Command("e621")]
		[Summary("get posts from e621.net (NSFW)")]
		[RequireContext(ContextType.Guild)]
		[RequireNsfw]
		public async Task E621GuildCommand(
			[Summary("the number of posts to request (default: 5)")]
			int count = 5,
			[Remainder] [Summary("the tags to be searched (default: none)")]
			string tags = "")
		{
			await E621Command(count, tags, false);
		}

		[Command("e621")]
		[Summary("get posts from e621.net (NSFW)")]
		[RequireContext(ContextType.DM | ContextType.Group)]
		public async Task E621DmOrGroupCommand(
			[Summary("the number of posts to request (default: 5)")]
			int count = 5,
			[Remainder] [Summary("the tags to be searched (default: none)")]
			string tags = "")
		{
			await E621Command(count, tags, false);
		}

		private async Task E621Command(int count, string tags, bool safeMode)
		{
			await ReplyAsync("Here you go, human:");
			await GetPostUrls(safeMode, tags, count)
				.SendMany(async msg => await ReplyAsync(msg));
		}

		[Command("e926")]
		[Summary("get posts from e926.net (SFW)")]
		public async Task E926Command(
			[Summary("the number of posts to request (default: 5)")]
			int count = 5,
			[Remainder] [Summary("the tags to be searched (default: none)")]
			string tags = "")
		{
			await E621Command(count, tags, true);
		}

		[Command("selfie")]
		[Alias("s")]
		[Summary("Get a random image of me")]
		public async Task SelfieCommand([Summary("minimum e926 score (default: 10)")]
			int minScore = 10)
		{
			var posts = await GetPostUrls(true, $"puro_(changed) solo score:>{minScore}", 1)
				.ToListAsync();

			var url = posts.FirstOrDefault();
			if (url is null)
			{
				await ReplyAsync("My camera is broken... Sorry Human.");
				return;
			}

			await ReplyAsync(url);
		}

		private static IAsyncEnumerable<string?> GetPostUrls(bool safeMode, string tags, int count)
		{
			var client = new SodiumClient(safeMode, UserAgent);
			var urls = client.GetPostsAsync(tags, count)
				.Select(p => p.File.Url?.AbsoluteUri)
				.Where(u => !string.IsNullOrWhiteSpace(u));
			return urls;
		}
	}
}