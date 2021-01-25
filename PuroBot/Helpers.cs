using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		public static IEnumerable<string> GetFileTree(this DirectoryInfo d, string fileSearchPattern = "",
			bool ignoreEmptyDirs = true, bool includeExtensions = false)
		{
			var subDirs = d.GetDirectories();
			Array.Sort(subDirs, (a, b) => Comparer.Default.Compare(a.Name, b.Name));
			var files = d.GetFiles(fileSearchPattern);
			Array.Sort(files, (a, b) => Comparer.Default.Compare(a.Name, b.Name));

			if (subDirs.Length == 0 && files.Length == 0 && ignoreEmptyDirs) yield return "";

			yield return d.Name;

			foreach (var subDir in subDirs)
			{
				var subTree = subDir.GetFileTree(fileSearchPattern, ignoreEmptyDirs);
				for (var i = 0; i < subTree.Count(); i++)
					if (i == 0)
						yield return "├ " + subTree.ElementAt(i);
					// else if (i == subTree.Count() - 1)
					// 	yield return " " + subTree.ElementAt(i);
					else
						yield return "| " + subTree.ElementAt(i);
			}

			for (var i = 0; i < files.Length; i++)
			{
				var filename = includeExtensions ? files[i].Name : Path.GetFileNameWithoutExtension(files[i].FullName);
				var symbol = i == files.Length - 1 ? "└ " : "├ ";
				yield return symbol + filename;
			}
		}
	}
}