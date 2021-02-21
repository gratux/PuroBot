using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using PuroBot.Extensions;
using u32char = System.Int32;
using u32string = System.Collections.Generic.List<int>;

// ReSharper disable BuiltInTypeReferenceStyle

namespace PuroBot.Services
{
	// bisqwit's speech synthesizer, ported to c#
	public partial class SpeechService
	{
		private const int SampleRate = 48000;
		private const int MaxOrder = 256;

		private const double FrameTimeFactor = 0.01d;
		private const double TokenPitch = 90.0d;
		private const double Volume = 0.7d;
		private const double BreathBaseLevel = 0.4d;
		private const double VoiceBaseLevel = 1.0d;

		private static readonly Random Rnd = new();

		private readonly List<byte> _audio = new();

		private readonly Dictionary<double, double> _last = new();
		private double[] _bp;
		private int _count;
		private int _offset;

		private double _sampleAverage;

		private static bool IsVowel(u32char c) => Vowels.Contains(c);

		private static bool IsAlphabet(u32char c) => Alphabet.Contains(c);

		private static bool IsConsonant(u32char c) => Consonants.Contains(c);

		public async Task<byte[]> SynthesizeMessageAsync(string message)
		{
			var phonemes = await PhonemizeAsync(message);
			var audio = await Vocalize(phonemes);
			return audio.ToArray();
		}

		private async Task<IEnumerable<byte>> Vocalize(List<ProsodyElement> phonemes)
		{
			InitVocalize();

			var frameTimeFactor = FrameTimeFactor;
			var tokenPitch = TokenPitch;
			var volume = Volume;
			var breathBaseLevel = BreathBaseLevel;
			var voiceBaseLevel = VoiceBaseLevel;

			foreach (var e in phonemes)
			{
				if (!Records.TryGetValue(e.Record, out var record) && !Records.TryGetValue('-', out record))
				{
					await LoggingService.Log(LogSeverity.Warning, nameof(SpeechService),
						$"Didn't find {char.ConvertFromUtf32(e.Record)}");
					continue;
				}

				tokenPitch = Math.Clamp(tokenPitch + Rnd.NextDouble() - 0.5d, 50.0d, 170.0d);
				breathBaseLevel = Math.Clamp(breathBaseLevel + (Rnd.NextDouble() - 0.5d) * 0.02d, 0.1d, 1.0d);
				voiceBaseLevel = Math.Clamp(voiceBaseLevel + (Rnd.NextDouble() - 0.5d) * 0.02d, 0.7d, 1.0d);

				var frameTime = SampleRate * frameTimeFactor;
				var alts = record.Data;
				var samplesCount = (int) (e.FramesCount * frameTime);

				var breath = breathBaseLevel + (1.0d - breathBaseLevel) * (1.0d * record.Voice);
				var buzz = record.Voice * voiceBaseLevel;
				var pitch = e.RelativePitch * tokenPitch;

				var doSynthFunc =
					new Func<Frame, int, int>((frame, count) =>
					{
						_audio.AddRange(Synthesize(frame, volume, breath, buzz, pitch, SampleRate, count));
						return count;
					});

				switch (record.Mode)
				{
					case RecordModes.ChooseRandomly:
						doSynthFunc(alts[Rnd.Next() % alts.Length], samplesCount);
						break;
					case RecordModes.Trill:
						for (var n = 0; samplesCount > 0; n++)
							samplesCount -= doSynthFunc(alts[n % alts.Length],
								Math.Min(samplesCount, (int) (frameTime * 1.5d)));
						break;
					case RecordModes.PlayInSequence:
						for (var a = 0; a < alts.Length; a++)
							doSynthFunc(alts[a],
								(a + 1) * samplesCount / alts.Length - a * samplesCount / alts.Length);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			return _audio;
		}

		private void InitVocalize()
		{
			_sampleAverage = 0;
			_bp = Enumerable.Repeat(0.0d, MaxOrder).ToArray();
			_offset = 0;
			_count = 0;
			_audio.Clear();
		}

		private IEnumerable<byte> Synthesize(Frame frame, double volume, double breath, double buzz, double pitch,
			uint sampleRate,
			int count)
		{
			var coefficients = new double[MaxOrder];
			frame.Coefficients.CopyTo(coefficients, 0);

			var pGain = frame.Gain;
			var pPitch = pitch;
			var pBreath = breath;
			var pBuzz = buzz;

			var hysteresis = -10;

			var slice = new List<double>();
			uint retriesBroken = 0, retriesClipping = 0;
			double amplitude = 0, amplitudeLimit = 7000;

			bool retry;
			do
			{
				//TODO: does this work?
				retry = false;
				slice.Clear();

				for (var n = 0; n < count; n++)
				{
					pitch = GetWithHysteresis(pPitch, -10);
					var gain = GetWithHysteresis(pGain, hysteresis);
					hysteresis = (int) -Math.Clamp(8.0d + Math.Log10(gain), 1.0d, 6.0d);

					_count++;
					var w = (_count %= (int) (sampleRate / pitch)) / (sampleRate / pitch);
					var f = (-0.5d + Rnd.NextDouble()) * GetWithHysteresis(pBreath, -12)
					        + (Math.Pow(2, w) - 1 / (1 + w)) * GetWithHysteresis(pBuzz, -12);
					// if (!double.IsFinite(f))
					// 	throw new Exception($"{nameof(f)} is not finite");

					var sum = f;
					for (var j = 0; j < frame.Coefficients.Length; j++)
						sum -= GetWithHysteresis(coefficients[j], hysteresis) *
						       _bp[(_offset + MaxOrder - j) % MaxOrder];

					if (!double.IsFinite(sum) || double.IsNaN(sum))
					{
						sum = 0;

						retriesBroken++;
						hysteresis = -1;
						_sampleAverage = 0;

						if (retriesBroken < 10)
						{
							retry = true;
							break;
						}
					}

					_offset++;
					var r = _bp[_offset %= MaxOrder] = sum;
					var sample = r * Math.Sqrt(GetWithHysteresis(pGain, hysteresis)) * (Int16.MaxValue + 1);

					_sampleAverage = _sampleAverage * 0.9993d + sample * (1 - 0.9993d);
					sample -= _sampleAverage;
					amplitude = n == 0 ? Math.Abs(sample) : amplitude * 0.99d + Math.Abs(sample) * 0.01d;
					if (n == 100 && amplitude > amplitudeLimit)
					{
						retriesClipping++;
						amplitudeLimit += 500;

						if (retriesClipping < 100)
						{
							retry = true;
							break;
						}
					}

					slice.Add(Math.Clamp(sample, Int16.MinValue, Int16.MaxValue));
					slice.Add(Math.Clamp(sample, Int16.MinValue, Int16.MaxValue));
				}
			} while (retry);

			return slice.SelectMany(BitConverter.GetBytes);
		}

		private double GetWithHysteresis(double value, int speed)
		{
			var newValue = value;
			if (!_last.ContainsKey(value))
			{
				_last.Add(value, newValue);
				return newValue;
			}

			var newFac = Math.Pow(2, speed);
			var oldFac = 1 - newFac;
			var oldValue = _last[value];
			return _last[value] = oldValue * oldFac + newValue * newFac;
		}

		private static async Task<List<ProsodyElement>> PhonemizeAsync(string text)
		{
			var input = await CanonizeAsync(text);

			var syllableId = new int[input.Count];

			var currentSyllable = 1;
			for (var position = 0; position < input.Count; currentSyllable++)
				position = AssignSyllableId(position, input, syllableId, ref currentSyllable);

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
							await LoggingService.Log(LogSeverity.Debug, nameof(SpeechService),
								$"Skipped unknown char: '{char.ConvertFromUtf32(input[pos])}' ({input[pos]})"
							);
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
			await LoggingService.Log(LogSeverity.Debug, nameof(SpeechService),
				$"Before: {result.ToUtf8()}");

			ApplyAccentRules(result, accentRules);

			await LoggingService.Log(LogSeverity.Debug, nameof(SpeechService),
				$"After: {result.ToUtf8()}");
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
				while (ApplyCanonizationRulesAt(position, result, rules))
					//try to apply more rules at the same position
					position -= Math.Min(position, 3);
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

		private class Record
		{
			public Record(RecordModes mode, double voice, Frame[] data)
			{
				Mode = mode;
				Voice = voice;
				Data = data;
			}

			public RecordModes Mode { get; }
			public double Voice { get; }
			public Frame[] Data { get; }
		}

		private class Frame
		{
			public Frame(double gain, double[] coefficients)
			{
				Gain = gain;
				Coefficients = coefficients;
			}

			public double Gain { get; }
			public double[] Coefficients { get; }
		}
	}
}