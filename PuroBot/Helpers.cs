using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace PuroBot
{
	public static class Helpers
	{
		private static readonly Semaphore Semaphore = new Semaphore(1, 1);

		public static async Task SendMany(List<string> messages, Func<string, Task<DiscordMessage>> sendMsgFunc)
		{
			Semaphore.WaitOne();
			var msgChunks = SplitList(messages);
			try
			{
				foreach (var chunk in msgChunks)
				{
					var msg = string.Join('\n', chunk);
					await sendMsgFunc.Invoke(msg);
					//TODO: rate limit
				}
			}
			finally
			{
				Semaphore.Release();
			}
		}

		private static IEnumerable<List<T>> SplitList<T>(List<T> list, int size = 5)
		{
			for (var i = 0; i < list.Count; i += size) yield return list.GetRange(i, Math.Min(size, list.Count - i));
		}

		// public static bool StartsWithAny(this string s, params char[] c)
		// {
		// 	return c.Any(s.StartsWith);
		// }

		public static bool StartsWithAny(this string s, IEnumerable<string> c)
		{
			return StartsWithAny(s, c, out _);
		}

		public static bool StartsWithAny(this string s, IEnumerable<string> c, out bool endsAfterStart)
		{
			foreach (var start in c)
			{
				if (!s.StartsWith(start)) continue;

				endsAfterStart = s.Length <= start.Length;
				return true;
			}

			endsAfterStart = false;
			return false;
		}
	}
}