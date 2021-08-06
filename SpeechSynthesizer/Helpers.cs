using System.Collections.Generic;
using System.Linq;
using PuroBot.Common;

namespace Bisqwit.SpeechSynthesizer
{
    public static class Helpers
    {
        private static readonly Utf32String Vowels = Utf32String.FromUtf16("aeiouyäüö");
        private static readonly Utf32String Consonants = Utf32String.FromUtf16("bcdfghjklmnpqrstvwxz");
        private static readonly Utf32String Alphabet = Utf32String.FromUtf16("abcdefghijklmnopqrstuvwxyz");

        public static bool IsAlphabet(Utf32Char? c)
        {
            return c is not null && Alphabet.Contains(c);
        }

        public static bool IsConsonant(Utf32Char? c)
        {
            return c is not null && Consonants.Contains(c);
        }

        public static bool IsSyllableDelimiter(Utf32Char c)
        {
            return c == '>' || c == '<' || c == '|';
        }

        public static bool IsVowel(Utf32Char? c)
        {
            return c is not null && Vowels.Contains(c);
        }

        public static bool IsWhitespace(Utf32Char c)
        {
            return c == ' ' || c == '\r' || c == '\v' || c == '\n' || c == '\t';
        }

        public static bool Matches(this Utf32String lhs, Utf32String rhs, int startIdx, int length)
        {
            var lhsSub = lhs.Skip(startIdx).Take(length).ToList();
            if (lhsSub.Count != rhs.Count)
                return false;

            // return false if any characters dont match
            return !lhsSub.Where((element, idx) => !element.Equals(rhs[idx])).Any();
        }

        public static void Replace<T>(this List<T> src, int startIndex, int count, List<T> replacement)
        {
            src.RemoveRange(startIndex, count);
            src.InsertRange(startIndex, replacement);
        }

        public static IEnumerable<T> ReverseNotInPlace<T>(this IReadOnlyList<T> src)
        {
            for (int i = src.Count - 1; i >= 0; i--)
                yield return src[i];
        }

        public static T? TryGetNext<T>(this IEnumerator<T> enumerator)
        {
            return enumerator.MoveNext() ? enumerator.Current : default;
        }

        public static bool TryGetRecord(Dictionary<Utf32Char, Record> records, Utf32Char recordChar, out Record? record)
        {
            if (records.TryGetValue(recordChar, out record) || records.TryGetValue('-', out record))
                return true;

            Logging.Error($"Didn't find {recordChar}");
            return false;
        }
    }
}
