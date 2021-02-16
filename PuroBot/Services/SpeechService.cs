using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using PuroBot.Extensions;
using PuroBot.StaticServices;
using u32char = System.Int32;
using u32string = System.Collections.Generic.List<int>;

// ReSharper disable BuiltInTypeReferenceStyle

namespace PuroBot.Services
{
	// bisqwit's speech synthesizer, ported to c#
	public partial class SpeechService
	{
		private static readonly Random Rnd = new();

		private static bool IsVowel(u32char c) => Vowels.Contains(c);

		private static bool IsAlphabet(u32char c) => Alphabet.Contains(c);

		private static bool IsConsonant(u32char c) => Consonants.Contains(c);

		public async Task<byte[]> SynthesizeMessageAsync(string message)
		{
			var phonemes = await PhonemizeAsync(message);
			var audio = Vocalize(phonemes);
			return audio.ToArray();
		}

		private static IEnumerable<byte> Vocalize(List<ProsodyElement> phonemes) => throw new NotImplementedException();

		private static async Task<List<ProsodyElement>> PhonemizeAsync(string text)
		{
			var input = await CanonizeAsync(text);

			var syllableId = new int[input.Count];

			var currentSyllable = 1;
			for (var position = 0; position < input.Count; currentSyllable++)
			{
				position = AssignSyllableId(position, input, syllableId, ref currentSyllable);
			}

			var pitchCurve = Enumerable.Repeat(1.0d, currentSyllable).ToList();
			var begin = 0;
			var end = syllableId.Length - 1;
			double first = 1.0d, last = 1.0d, midPos = 1.0d;

			for (var i = 0; i < input.Count; i++)
			{
				if (input[i] == '<' || input[i] == '|')
				{
					// Find the matching '>', or '|'
					begin = i;
					end = input.IndexOfAny(i + 1, '>', '|');
					if (end == -1) end = input.Count;
					first = input[i] == '<' ? 1.4d : 1.25d;

					//end may be one above the array bounds, treat as null terminator in that case
					last = input.ElementAtOrDefault(end) == '|' ? 1.0d : 0.86d;
					midPos = 0.7d + Rnd.NextDouble() * 0.1d;

					//afterwards, end is definitely inbounds
					end--;
				}

				// Interpolate pitch between the syllables that have '<' or '|' and those that have '>' or '|'
				var fltPos = midPos;
				if (syllableId[i] == syllableId[begin]) fltPos = 0;
				if (syllableId[i] == syllableId[end]) fltPos = 0.95d + Rnd.NextDouble() * 0.1d;
				fltPos += Rnd.NextDouble() * 0.2d;
				pitchCurve[syllableId[i]] = first + (last - first) * fltPos;
			}

			var elements = new List<ProsodyElement>();
			for (var pos = 0; pos < input.Count; pos++)
			{
				var endPos = pos;
				var repLenFunc = new Func<uint>(() =>
				{
					uint r = 0;
					for (; input[endPos] == input.ElementAtOrDefault(endPos + 1); r++)
						endPos++;
					return r;
				});

				var nextElement = input.ElementAtOrDefault(pos + 1);
				var surroundedByVowel = nextElement != input[pos] && IsVowel(nextElement) ||
				                        pos > 0 && IsVowel(input[pos - 1]);

				if (Maps.ContainsKey(input[pos]))
					elements.AddRange(Maps[input[pos]].Select(e => new ProsodyElement
						(
							e.Character,
							e.Length + e.RepeatedLength * repLenFunc() + (surroundedByVowel ? e.SurroundedLength : 0),
							pitchCurve[syllableId[pos]]
						)
					));
				else
					switch (input[pos])
					{
						case ' ':
						case '\r':
						case '\v':
						case '\n':
						case '\t':
							// Whitespace. If the previous character was a vowel
							// and the next one is a vowel as well, add a glottal stop.
							if (pos > 0 && IsVowel(input[pos - 1]) && IsVowel(nextElement) || input[pos] != ' ')
							{
								elements.Add(new ProsodyElement('-', 3, pitchCurve[syllableId[pos]]));
								elements.Add(new ProsodyElement('q', 1, pitchCurve[syllableId[pos]]));
							}

							break;
						case '>':
						case '<':
						case '|':
							//syllable delimiters
							break;
						default:
							await LoggingService.LogAsync(new LogMessage(LogSeverity.Debug, nameof(SpeechService),
								$"Skipped unknown char: '{char.ConvertFromUtf32(input[pos])}' ({input[pos]})"
							));
							break;
					}

				pos = endPos;
			}

			return elements;
		}

		private static int AssignSyllableId(int position, u32string input, int[] syllableId, ref int currentSyllable)
		{
			var isEndPunctuation = new Func<u32char, bool>(">¯q\"".ToUtf32().Contains);

			while (position < input.Count && !IsAlphabet(input[position]))
				syllableId[position++] = currentSyllable++;
			while (position < input.Count && IsConsonant(input[position]))
				syllableId[position++] = currentSyllable;
			while (position < input.Count && IsVowel(input[position]))
				syllableId[position++] = currentSyllable;
			while (position < input.Count && IsConsonant(input[position]) && !IsVowel(input[position + 1]))
				syllableId[position++] = currentSyllable;
			while (position < input.Count && isEndPunctuation(input[position]))
				syllableId[position++] = currentSyllable;
			return position;
		}

		private static async Task EnglishConvertAsync(u32string text)
		{
			// alphabetically sorted list of rules
			// each letter can have multiple rules of four strings each
			// left, middle, right part and the replacement middle value
			var rules = new List<u32string[]>[26 + 1];

			var regex = new Regex(@"(.*?)\|(.*?)\|(.*?)\|(.*?),");
			var patternMatches = regex.Matches(Patterns).Select(m => m.Groups.Values.Skip(1)).ToArray();
			foreach (var patternMatch in patternMatches)
			{
				var row = patternMatch.Select(g => g.Value.ToUtf32()).ToArray();
				var ruleIdx = IsAlphabet(row[1][0]) ? 1 + (row[1][0] - 'a') : 0;
				rules[ruleIdx] ??= new List<u32string[]>();
				rules[ruleIdx].Add(row);
			}

			var result = new u32string();
			for (var position = 1; position < text.Count;)
				position = ApplyConversionRules(text, position, rules, result);

			await ChangeAccentAsync(result, FinnishAccentTab);
		}

		private static int ApplyConversionRules(u32string text, int position, List<u32string[]>[] rules,
			u32string result)
		{
			var ruleMatched = false;
			var ruleIdx = IsAlphabet(text[position]) ? 1 + (text[position] - 'a') : 0;
			foreach (var rule in rules[ruleIdx])
			{
				u32string lPattern = rule[0], mPattern = rule[1], rPattern = rule[2];

				var lMatch = MatchesRulePattern(
					rPattern,
					text.GetEnumerator());
				var mMatch = text.Matches(mPattern, position, mPattern.Count);
				var rMatch = MatchesRulePattern(
					lPattern,
					text.ReverseNotInPlace().GetEnumerator()
				);

				// current rule doesn't match, try next
				if (!lMatch || !mMatch || !rMatch) continue;

				result.AddRange(rule[3]);
				position += mPattern.Count;
				ruleMatched = true;
				break;
			}

			if (!ruleMatched)
				result.Add(text[position++]);

			return position;
		}

		//TODO: refactor to reduce method complexity
		private static bool MatchesRulePattern(u32string pattern, IEnumerator<u32char> textEnumerator)
		{
			//TODO: compare strings as strings instead of each char separately
			foreach (var patternChar in pattern)
				switch (patternChar)
				{
					case '#':
					{
						uint n = 0;
						for (; IsVowel(textEnumerator.Current); n++)
							textEnumerator.MoveNext();
						if (n == 0)
							return false;
						break;
					}
					case ':':
						while (IsConsonant(textEnumerator.Current)) textEnumerator.MoveNext();
						break;
					case '^':
						if (!IsConsonant(textEnumerator.TryGetNext()))
							return false;
						break;
					case '.':
						if (!"bdvgjlmnrwz".ToUtf32().Contains(textEnumerator.TryGetNext()))
							return false;
						break;
					case '+':
						if (!"eyi".ToUtf32().Contains(textEnumerator.TryGetNext()))
							return false;
						break;
					case '%':
					{
						var c = textEnumerator.TryGetNext();
						if (c == 'i')
						{
							if (textEnumerator.TryGetNext() != 'n' || textEnumerator.TryGetNext() != 'g')
								return false;
							break;
						}

						if (c != 'e')
							return false;

						var c2 = textEnumerator.TryGetNext();
						if (c2 == 'l')
						{
							if (textEnumerator.TryGetNext() != 'y')
								return false;
							break;
						}

						if (c2 != 'r' && c2 != 's' && c2 != 'd')
							return false;
						break;
					}
					case ' ':
					{
						var c = textEnumerator.TryGetNext();
						if (IsAlphabet(c) || c == '\'')
							return false;
						break;
					}
					default:
						if (patternChar != textEnumerator.TryGetNext())
							return false;
						break;
				}

			return true;
		}

		private static async Task ChangeAccentAsync(u32string result, Dictionary<u32string, u32string> accentRules)
		{
			// At this point, the result string is pretty much an ASCII representation of IPA.
			// Now just touch up it a bit to convert it into typical Finnish pronunciation.
			await LoggingService.LogAsync(new LogMessage(LogSeverity.Debug, nameof(SpeechService),
				$"Before: {result.ToUtf8()}"));

			ApplyAccentRules(result, accentRules);

			await LoggingService.LogAsync(new LogMessage(LogSeverity.Debug, nameof(SpeechService),
				$"After: {result.ToUtf8()}"));
		}

		private static void ApplyAccentRules(u32string result, Dictionary<u32string, u32string> rules)
		{
			for (var position = 0; position < result.Count; position++)
				position = ApplyAccentRulesAt(position, result, rules);
		}

		private static int ApplyAccentRulesAt(int position, u32string result, Dictionary<u32string, u32string> rules)
		{
			foreach (var (initial, replacement) in rules)
			{
				if (!result.Matches(initial, position, initial.Count)) continue;

				result.Replace(position, initial.Count, replacement);
				position += replacement.Count - 1;
				break;
			}

			return position;
		}

		private static void ApplyCanonizationRules(u32string result, Dictionary<u32string, u32string> rules)
		{
			for (var position = 0; position < result.Count; position++)
			{
				while (ApplyCanonizationRulesAt(position, result, rules))
				{
					//try to apply more rules at the same position
					position -= Math.Min(position, 3);
				}
			}
		}

		private static bool ApplyCanonizationRulesAt(int position, u32string result,
			Dictionary<u32string, u32string> rules)
		{
			var rulesApplied = false;

			foreach (var (initial, replacement) in rules)
			{
				if (!result.Matches(initial, position, initial.Count)) continue;

				result.Replace(position, initial.Count, replacement);
				rulesApplied = true;
				break;
			}

			return rulesApplied;
		}

		private static async Task<u32string> CanonizeAsync(string text)
		{
			var canonized = text.ToLowerInvariant().ToUtf32();
			ApplyCanonizationRules(canonized, SymbolCanon);

			//pad input with leading and trailing space, so all patterns can match
			canonized.Insert(0, ' ');
			canonized.Add(' ');

			await EnglishConvertAsync(canonized);
			ApplyCanonizationRules(canonized, PunctuationCanon);
			return canonized;
		}

		private enum RecordModes
		{
			ChooseRandomly,
			Trill,
			PlayInSequence
		}

		private struct ProsodyElement
		{
			public ProsodyElement(u32char record, uint framesCount, double relativePitch)
			{
				Record = record;
				FramesCount = framesCount;
				RelativePitch = relativePitch;
			}

			public u32char Record { get; }
			public uint FramesCount { get; }
			public double RelativePitch { get; }
		}

		private readonly struct PhonemeMapItem
		{
			public PhonemeMapItem(int character, uint length, uint repeatedLength, uint surroundedLength)
			{
				Character = character;
				Length = length;
				RepeatedLength = repeatedLength;
				SurroundedLength = surroundedLength;
			}

			public u32char Character { get; }
			public uint Length { get; }
			public uint RepeatedLength { get; }
			public uint SurroundedLength { get; }
		}
	}
}