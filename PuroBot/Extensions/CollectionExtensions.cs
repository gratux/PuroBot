using System;
using System.Collections.Generic;
using System.Linq;

namespace PuroBot.Extensions
{
	public static class CollectionExtensions
	{
		public static async IAsyncEnumerable<T[]> Partition<T>(this IAsyncEnumerable<T> list, int chunkSize = 5)
		{
			await using var enumerator = list.GetAsyncEnumerator();
			var chunk = new List<T>();

			for (var i = 0; await enumerator.MoveNextAsync(); i++)
			{
				chunk.Add(enumerator.Current);
				if ((i + 1) % chunkSize != 0) continue;

				yield return chunk.ToArray();
				chunk.Clear();
			}
		}

		public static bool Matches<T>(this IEnumerable<T> lhs, List<T> rhs, int startIdx, int length)
		{
			var lhsSub = lhs.Skip(startIdx).Take(length).ToList();
			if (lhsSub.Count != rhs.Count)
				return false;

			// return false if any characters dont match
			return !lhsSub.Where((element, idx) => !element.Equals(rhs[idx])).Any();
		}

		public static string IncludeIfAny(this IReadOnlyList<string> items, string header,
			Func<string, string> itemFormatter, string separator, bool startInNewLine = false)
		{
			if (items.Any())
				return $"{header.AsHeader()}: " + (startInNewLine ? "\n" : string.Empty) +
				       string.Join(separator, items.Select(itemFormatter.Invoke)) + "\n";
			return string.Empty;
		}

		public static T TryGetNext<T>(this IEnumerator<T> enumerator) =>
			enumerator.MoveNext() ? enumerator.Current : default;

		public static IEnumerable<T> ReverseNotInPlace<T>(this IReadOnlyList<T> src)
		{
			for (var i = src.Count - 1; i >= 0; i--)
				yield return src[i];
		}

		public static int IndexOfAny<T>(this List<T> src, int startIndex, params T[] items) =>
			src.FindIndex(startIndex, items.Contains);

		public static void Replace<T>(this List<T> src, int startIndex, int count, List<T> replacement)
		{
			src.RemoveRange(startIndex, count);
			src.InsertRange(startIndex, replacement);
		}

		public static IEnumerable<byte> NormalizeAudio(this IEnumerable<byte> src, double relativeVolume = 0.7)
		{
			return src.Select(v => v.Scale(sbyte.MinValue, sbyte.MaxValue, relativeVolume * sbyte.MinValue, relativeVolume * sbyte.MaxValue));
		}
	}
}