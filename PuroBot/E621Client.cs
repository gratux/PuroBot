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
		static readonly string UserAgent = $"PuroBot/{Assembly.GetExecutingAssembly().GetName().Version} (by d3r_5h06un)";
		static readonly Semaphore Sp = new Semaphore(1, 1);

		public E621Client() : base (UserAgent)
		{
		}

		public async Task<IList<string>> GetRandomPostsUrl(string tags, int minScore, int count)
		{
			Sp.WaitOne();
			var searchTask = Search(new E621SearchOptions()
			{
				Tags = $"{tags} order:random score:>={minScore}",
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