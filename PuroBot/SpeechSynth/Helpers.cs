using System.Collections.Generic;
using PuroBot.Logger;

namespace PuroBot.SpeechSynth
{
	public abstract class Helpers
	{
		private static readonly Utf32String Vowels = Utf32String.FromUtf16("aeiouyäüö");
		private static readonly Utf32String Consonants = Utf32String.FromUtf16("bcdfghjklmnpqrstvwxz");
		private static readonly Utf32String Alphabet = Utf32String.FromUtf16("abcdefghijklmnopqrstuvwxyz");

		public static bool IsVowel(Utf32Char c) => Vowels.Contains(c);

		public static bool IsAlphabet(Utf32Char c) => Alphabet.Contains(c);

		public static bool IsConsonant(Utf32Char c) => Consonants.Contains(c);
		
		public static bool IsWhitespace(Utf32Char c) => c == ' ' || c == '\r' || c == '\v' || c == '\n' || c == '\t';

		public static bool IsSyllableDelimiter(Utf32Char c) => c == '>' || c == '<' || c == '|';

		public static bool TryGetRecord(Utf32Char recordChar, Dictionary<Utf32Char, Record> records, out Record record, ILogger logger)
		{
			if (records.TryGetValue(recordChar, out record) || records.TryGetValue('-', out record)) return true;
			
			logger.LogError($"Didn't find {recordChar}");
			return false;

		}
	}
}