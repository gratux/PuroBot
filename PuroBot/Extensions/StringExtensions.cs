namespace PuroBot.Extensions
{
    public static class StringExtensions
    {
        public static string AsBold(this string text)
        {
            return $"**{text}**";
        }

        public static string AsCode(this string text, bool block = false, string language = "")
        {
            return block ? $"\n```{language}\n{text}\n```\n" : $"`{text}`";
        }

        public static string AsHeader(this string text)
        {
            return $"__{text}__";
        }

        public static string AsItalic(this string text)
        {
            return $"*{text}*";
        }

        public static string AsListItem(this string text)
        {
            return $"• {text}";
        }
    }
}
