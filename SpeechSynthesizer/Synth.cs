using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PuroBot.Common;
using static SpeechSynthesizer.SynthData;
using static SpeechSynthesizer.Helpers;

namespace SpeechSynthesizer
{
	/// <summary>bisqwit's speech synthesizer, ported to c#</summary>
	public class Synth
	{
		private const int SampleRate = 48000; //Hz

		private const double FrameTimeFactor = 0.01d;
		private const double TokenPitch = 100.0d;
		private const double Volume = 0.7d;
		private const double BreathBaseLevel = 0.4d;
		private const double VoiceBaseLevel = 1.0d;
		private const int MaxOrder = 256;

		private static readonly Random Rnd = new();

		private readonly double[] _bp = new double[MaxOrder];

		private readonly Dictionary<double, double> _last = new();
		private int _count, _offset;

		private double _pGain, _pPitch, _pBreath, _pBuzz;
		private double _sampleAverage;

		public IEnumerable<byte> SynthesizeText(string message)
		{
			var phonemes = Phonemize(message).ToList();
			var audio = VocalizePhonemes(phonemes).ToList();
			return audio;
		}

		private IEnumerable<byte> VocalizePhonemes(IEnumerable<ProsodyElement> phonemes)
		{
			InitVocalize();

			var tokenPitch = TokenPitch;
			var breathBaseLevel = BreathBaseLevel;
			var voiceBaseLevel = VoiceBaseLevel;

			var audio = new List<byte>();

			foreach (ProsodyElement e in phonemes)
			{
				if (!TryGetRecord(Records, e.RecordChar, out Record? record))
					continue;

				Debug.Assert(record is not null, "record is not null");

				tokenPitch = Math.Clamp(tokenPitch + Rnd.NextDouble() - 0.5d, 50.0d, 170.0d);
				breathBaseLevel = Math.Clamp(breathBaseLevel + (Rnd.NextDouble() - 0.5d) * 0.02d, 0.1d, 1.0d);
				voiceBaseLevel = Math.Clamp(voiceBaseLevel + (Rnd.NextDouble() - 0.5d) * 0.02d, 0.7d, 1.0d);

				const double frameTime = SampleRate * FrameTimeFactor;
				var frames = record.Data;
				var samplesCount = (int) (e.FramesCount * frameTime);

				var breath = breathBaseLevel + (1.0d - breathBaseLevel) * (1.0d * record.Voice);
				var buzz = record.Voice * voiceBaseLevel;
				var pitch = e.RelativePitch * tokenPitch;

				var doSynth = new Func<Frame, int, int>((frame, count) =>
				{
					audio.AddRange(Synthesize(frame, Volume, breath, buzz, pitch, SampleRate, count));
					return count;
				});

				switch (record.Mode)
				{
					case RecordModes.ChooseRandomly:
						doSynth(frames[Rnd.Next(frames.Length)], samplesCount);
						break;
					case RecordModes.Trill:
						for (var n = 0; samplesCount > 0; n++)
							samplesCount -= doSynth(frames[n % frames.Length],
								Math.Min(samplesCount, (int) (frameTime * 1.5d)));
						break;
					case RecordModes.PlayInSequence:
						foreach (Frame frame in frames)
							doSynth(frame, samplesCount / frames.Length);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			return audio;
		}

		private void InitVocalize()
		{
			_sampleAverage = 0;

			_count = 0;
			_offset = 0;
			Array.Clear(_bp, 0, _bp.Length);

			_last.Clear();
		}

		private IEnumerable<byte> Synthesize(Frame frame, double volume, double breath, double buzz, double pitch,
			uint sampleRate,
			int nSamples)
		{
			_pGain = frame.Gain;
			_pPitch = pitch;
			_pBreath = breath;
			_pBuzz = buzz;

			var hysteresis = -10;
			var slice = new List<double>();
			uint retries = 0, retries2 = 0;
			var amplitude = 0d;
			var amplitudeLimit = 7000d;

			while (true)
			{
				var broken = false;
				var clipping = false;

				for (var n = 0; n < nSamples; n++)
				{
					pitch = GetWithHysteresis(_pPitch, -10);
					var gain = GetWithHysteresis(_pGain, hysteresis);
					hysteresis = (int) -Math.Clamp(8d + Math.Log10(gain), 2d, 6d);

					_count++;
					var w = (_count %= (int) (sampleRate / pitch)) / (sampleRate / pitch);
					var f = (Rnd.NextDouble() - .5) * GetWithHysteresis(_pBreath, -12)
					        + (Math.Pow(2, w) - 1 / (1 + w)) * GetWithHysteresis(_pBuzz, -12);
					if (!double.IsFinite(f))
						throw new Exception("f is not finite");

					var sum = f;
					for (var j = 0; j < frame.Coefficients.Length; j++)
						sum -= GetWithHysteresis(frame.Coefficients[j], hysteresis) *
						       _bp[(_offset + MaxOrder - j) % MaxOrder];

					if (!double.IsFinite(sum) || double.IsNaN(sum))
					{
						broken = true;
						sum = 0;
					}

					_offset++;
					var r = _bp[_offset %= MaxOrder] = sum;

					var sample = r * Math.Sqrt(GetWithHysteresis(_pGain, hysteresis)) * short.MaxValue;

					_sampleAverage = _sampleAverage * .9993d + sample * (1 - .9993d);
					sample -= _sampleAverage;
					amplitude = n == 0 ? Math.Abs(sample) : amplitude * .99d + Math.Abs(sample) * .01d;
					if (n == 100 && amplitude > amplitudeLimit)
					{
						retries2++;
						amplitudeLimit += 500;
						slice.Clear();
						if (retries2 < 100)
							clipping = true;
						//clippingRetries maximum reached, compromise by generating clipping sample
					}

					slice.Add(Math.Clamp(sample, short.MinValue, short.MaxValue) * volume);
					slice.Add(Math.Clamp(sample, short.MinValue, short.MaxValue) * volume);
				}

				if (clipping) // retry if clipping
					continue;

				if (broken) // retry if generator broken
				{
					Array.Clear(_bp, 0, _bp.Length);
					slice.Clear();
					retries++;
					hysteresis = -1;
					_sampleAverage = 0;
					if (retries < 10) // if maximum not reached, retry from beginning
						continue;
				}

				break; // no retry triggered or maximum reached
			}

			return slice.SelectMany(s => BitConverter.GetBytes((short) s));
		}

		private double GetWithHysteresis(double value, int speed)
		{
			var newValue = value;
			if (!_last.TryGetValue(value, out var oldValue))
			{
				_last.Add(value, newValue);
				return newValue;
			}

			var newFac = Math.Pow(2, speed);
			var oldFac = 1 - newFac;
			return _last[value] = oldValue * oldFac + newValue * newFac;
		}

		private static IEnumerable<ProsodyElement> Phonemize(string text)
		{
			Utf32String input = Canonize(text);

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

			for (var pos = 0; pos < input.Count; pos++)
			{
				var endPos = pos;

				//get the length of repeated character chains
				var repLenFunc = new Func<uint>(() =>
				{
					uint r = 0;
					for (; input[endPos] == input.ElementAtOrDefault(endPos + 1); r++)
						endPos++;
					return r;
				});

				Utf32Char? nextElement = input.ElementAtOrDefault(pos + 1);
				var surroundedByVowel = nextElement != input[pos] && IsVowel(nextElement) ||
				                        pos > 0 && IsVowel(input[pos - 1]);

				if (Maps.ContainsKey(input[pos]))
					foreach (var element in Maps[input[pos]])
						yield return new ProsodyElement
						(
							element.Character,
							element.Length + element.RepeatedLength * repLenFunc() +
							(surroundedByVowel ? element.SurroundedLength : 0),
							pitchCurve[syllableId[pos]]
						);
				else
					switch (input[pos])
					{
						case { } c when IsWhitespace(c):
							// If the previous character was a vowel
							// and the next one is a vowel as well, add a glottal stop.
							if (pos > 0 && IsVowel(input[pos - 1]) && IsVowel(nextElement) || input[pos] != ' ')
							{
								yield return new ProsodyElement('-', 3, pitchCurve[syllableId[pos]]);
								yield return new ProsodyElement('q', 1, pitchCurve[syllableId[pos]]);
							}

							break;
						case { } c when IsSyllableDelimiter(c):
							break;
						default:
							Logging.Warn($"Skipped unknown char: '{input[pos]}' ({input[pos]})");
							break;
					}

				pos = endPos;
			}
		}

		private static int AssignSyllableId(int position, Utf32String input, int[] syllableId, ref int currentSyllable)
		{
			var isEndPunctuation = new Func<Utf32Char, bool>(Utf32String.FromUtf16(">¯q\"").Contains);

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

		private static void EnglishConvert(Utf32String text)
		{
			// alphabetically sorted list of rules
			// each letter can have multiple rules of four strings each
			// left, middle, right part and the replacement middle value
			var rules = new List<Utf32String[]>[26 + 1];

			foreach (var row in Patterns)
			{
				var ruleIdx = IsAlphabet(row[1][0]) ? 1 + (row[1][0] - 'a') : 0;
				rules[ruleIdx].Add(row);
			}

			var result = new Utf32String();
			for (var position = 1; position < text.Count;)
				position = ApplyConversionRules(text, position, rules, result);

			//ChangeAccentAsync(result, FinnishAccentTab);
			ChangeAccentAsync(result, EnglishAccentTab);

			text.Clear();
			text.AddRange(result);
		}

		private static int ApplyConversionRules(Utf32String text, int position, List<Utf32String[]>[] rules,
			Utf32String result)
		{
			var ruleMatched = false;
			var ruleIdx = IsAlphabet(text[position]) ? 1 + (text[position] - 'a') : 0;
			foreach (var rule in rules[ruleIdx])
			{
				Utf32String lPattern = rule[0], mPattern = rule[1], rPattern = rule[2];

				var lMatch = MatchesRulePattern(
					rPattern,
					text.GetEnumerator());
				var mMatch = text.Matches(mPattern, position, mPattern.Count);
				var rMatch = MatchesRulePattern(
					lPattern,
					text.Chars.ReverseNotInPlace().GetEnumerator()
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

		private static bool MatchesRulePattern(Utf32String pattern, IEnumerator<Utf32Char> textEnumerator)
		{
			var patternMatchers = new Dictionary<Utf32Char, Func<Utf32Char, bool>>
			{
				{'#', _ => IsVowelBlock(textEnumerator)},
				{':', _ => IsConsonantBlock(textEnumerator)},
				{'^', _ => IsConsonant(textEnumerator.TryGetNext())},
				{'.', _ => Utf32String.FromUtf16("bdvgjlmnrwz").Contains(textEnumerator.TryGetNext())},
				{'+', _ => Utf32String.FromUtf16("eyi").Contains(textEnumerator.TryGetNext())},
				{'%', _ => IsSpecialLetterCombination(textEnumerator)},
				{' ', _ => !IsAlphabetOrSingleQuote(textEnumerator)}
			};

			var defaultMatcher = new Func<Utf32Char, bool>(patternChar => patternChar == textEnumerator.TryGetNext());

			var results = pattern.Select(patternChar =>
				patternMatchers.GetValueOrDefault(patternChar, defaultMatcher).Invoke(patternChar));

			return results.All(result => result);
		}

		private static bool IsSpecialLetterCombination(IEnumerator<Utf32Char> textEnumerator)
		{
			Utf32Char? c = textEnumerator.TryGetNext();
			if (c == 'i') return textEnumerator.TryGetNext() == 'n' && textEnumerator.TryGetNext() == 'g';

			if (c != 'e')
				return false;

			Utf32Char? c2 = textEnumerator.TryGetNext();
			if (c2 == 'l') return textEnumerator.TryGetNext() == 'y';

			return c2 == 'r' || c2 == 's' || c2 == 'd';
		}

		private static bool IsAlphabetOrSingleQuote(IEnumerator<Utf32Char> textEnumerator)
		{
			Utf32Char? c = textEnumerator.TryGetNext();
			return IsAlphabet(c) || c == '\'';
		}

		private static bool IsConsonantBlock(IEnumerator<Utf32Char> textEnumerator)
		{
			while (IsConsonant(textEnumerator.Current)) textEnumerator.MoveNext();
			return true;
		}

		private static bool IsVowelBlock(IEnumerator<Utf32Char> textEnumerator)
		{
			uint n = 0;
			for (; IsVowel(textEnumerator.Current); n++) textEnumerator.MoveNext();
			return n != 0;
		}

		private static void ChangeAccentAsync(Utf32String result,
			Dictionary<Utf32String, Utf32String> accentRules)
		{
			// At this point, the result string is pretty much an ASCII representation of IPA.
			// Now just touch up it a bit to convert it into typical Finnish pronunciation.
			Logging.Debug($"Before: {result.ToUtf16()}");

			ApplyAccentRules(result, accentRules);

			Logging.Debug($"After: {result.ToUtf16()}");
		}

		private static void ApplyAccentRules(Utf32String result, Dictionary<Utf32String, Utf32String> rules)
		{
			for (var position = 0; position < result.Count; position++)
				position = ApplyAccentRulesAt(position, result, rules);
		}

		private static int ApplyAccentRulesAt(int position, Utf32String result,
			Dictionary<Utf32String, Utf32String> rules)
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

		private static void ApplyCanonizationRules(Utf32String result, Dictionary<Utf32String, Utf32String> rules)
		{
			for (var position = 0; position < result.Count; position++)
				while (ApplyCanonizationRulesAt(position, result, rules))
					//try to apply more rules at the same position
					position -= Math.Min(position, 3);
		}

		private static bool ApplyCanonizationRulesAt(int position, Utf32String result,
			Dictionary<Utf32String, Utf32String> rules)
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

		private static Utf32String Canonize(string text)
		{
			Utf32String canonized = Utf32String.FromUtf16(text.ToLowerInvariant());
			ApplyCanonizationRules(canonized, SymbolCanon);

			//pad input with leading and trailing space, so all patterns can match
			canonized.Insert(0, ' ');
			canonized.Add(' ');

			EnglishConvert(canonized);
			ApplyCanonizationRules(canonized, PunctuationCanon);
			return canonized;
		}
	}
}