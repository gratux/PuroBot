using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuroBot.Extensions
{
	public static class StringExtensions
	{
		public static string AsHeader(this string text) => $"__{text}__";

		public static string AsBold(this string text) => $"**{text}**";

		public static string AsItalic(this string text) => $"*{text}*";

		public static string AsCode(this string text, bool block = false, string language = "") =>
			block ? $"```{language}\n{text}\n```" : $"`{text}`";

		public static string AsListItem(this string text) => $"• {text}";

		public static List<int> ToUtf32(this string s)
		{
			var converted = new List<int>();
			for (var i = 0; i < s.Length; i++)
			{
				var unicodeCodePoint = char.ConvertToUtf32(s, i);
				if (unicodeCodePoint > 0xffff) i++;

				converted.Add(unicodeCodePoint);
			}

			return converted;
		}

		public static string ToUtf8(this IEnumerable<int> s) => string.Concat(s.Select(char.ConvertFromUtf32));
	}
}