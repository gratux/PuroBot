namespace PuroBot.Extensions
{
	public static class StringExtensions
	{
		public static string AsHeader(this string text) => $"__{text}__";

		public static string AsBold(this string text) => $"**{text}**";

		public static string AsItalic(this string text) => $"*{text}*";

		public static string AsCode(this string text, bool block = false, string language = "") =>
			block ? $"\n```{language}\n{text}\n```\n" : $"`{text}`";

		public static string AsListItem(this string text) => $"• {text}";
	}
}