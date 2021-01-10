using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace PuroBot
{
	public static class Helpers
	{
		public static async Task SendMany(List<string> messages, Func<string, Task<DiscordMessage>> sendMsgFunc)
		{
			var msgChunks = SplitList(messages);
			foreach (var chunk in msgChunks)
			{
				var msg = string.Join('\n', chunk);
				await sendMsgFunc.Invoke(msg);
				//TODO: rate limit
			}
		}

		private static IEnumerable<List<T>> SplitList<T>(List<T> list, int size = 5)
		{
			for (var i = 0; i < list.Count; i += size) yield return list.GetRange(i, Math.Min(size, list.Count - i));
		}
	}
}