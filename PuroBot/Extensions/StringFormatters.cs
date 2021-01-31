namespace PuroBot.Extensions
{
	public static class StringFormatters
	{
		public static string AsHeader(this string text) => $"__{text}__";

		public static string AsBold(this string text) => $"**{text}**";

		public static string AsItalic(this string text) => $"*{text}*";

		public static string AsCode(this string text, bool block = false, string language = null) =>
			block ? $"```{language}\n{text}\n```" : $"`{text}`";

		public static string AsListItem(this string text) => $"• {text}";
	}
}