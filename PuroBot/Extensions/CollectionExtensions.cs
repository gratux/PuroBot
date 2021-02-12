using System;
using System.Collections.Generic;
using System.Linq;

namespace PuroBot.Extensions
{
	public static class CollectionExtensions
	{
		public static IEnumerable<T[]> Partition<T>(this IEnumerable<T> list, int chunkSize = 5)
		{
			using var enumerator = list.GetEnumerator();
			var chunk = new List<T>();

			for (var i = 0; enumerator.MoveNext(); i++)
			{
				chunk.Add(enumerator.Current);
				if ((i + 1) % chunkSize != 0) continue;

				yield return chunk.ToArray();
				chunk.Clear();
			}
		}

		public static bool Match<T>(this IEnumerable<T> lhs, List<T> rhs, int startIdx, int length)
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
				return $"{header.AsHeader()}: " + (startInNewLine ? "\n" : null) +
				       string.Join(separator, items.Select(itemFormatter.Invoke)) + "\n";
			return null;
		}

		public static T TryGetNext<T>(this IEnumerator<T> enumerator) =>
			enumerator.MoveNext() ? enumerator.Current : default;

		public static IEnumerable<T> ReverseNotInPlace<T>(this IReadOnlyList<T> src)
		{
			for (var i = src.Count - 1; i >= 0; i--)
				yield return src[i];
		}
	}
}