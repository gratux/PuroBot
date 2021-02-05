using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using E621;

namespace PuroBot
{
	public class E621Client : E621.E621Client
	{
		private static readonly string UserAgent =
			$"PuroBot/{Assembly.GetExecutingAssembly().GetName().Version} (by d3r_5h06un)";

		private static readonly SemaphoreSlim Sp = new SemaphoreSlim(1);

		public E621Client() : base(UserAgent)
		{
		}

		public async Task<List<string>> GetPostUrls(string tags, int count, int minScore = 10)
		{
			await Sp.WaitAsync();
			var searchTask = Search(new E621SearchOptions
			{
				Tags = $"{tags} score:>={minScore}",
				Limit = count
			});

			try
			{
				await searchTask;
				var posts = searchTask.Result;
				return new List<string>(posts.Select(p => p.File.Url));
			}
			catch
			{
				return new List<string>();
			}
			finally
			{
				Sp.Release();
			}
		}
	}
}