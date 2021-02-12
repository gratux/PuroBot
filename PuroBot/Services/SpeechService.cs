using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Discord;
using PuroBot.Extensions;
using PuroBot.StaticServices; //typedef for clearer intent
using u32char = System.Int32;
using u32string = System.Collections.Generic.List<int>;

// ReSharper disable BuiltInTypeReferenceStyle

namespace PuroBot.Services
{
	// bisqwit's speech synthesizer, ported to c#
	public class SpeechService
	{
		private const string Patterns =
			".|'s||z,#:.e|'s||z,#|'s||z,|'||,|a| |@, |are| |0r, |ar|o|@r,|ar|#|er,^|as|#|eIs,|a|wa|@,|aw||O, :|any||eni," +
			"|a|^+#|eI,#:|ally||@li, |al|#|@l,|again||@gen,#:|ag|e|IdZ,|a|^+:#|&, :|a|^+ |eI,|a|^%|eI, |arr||@r,|arr||&r" +
			", :|ar| |0r,|ar| |3,|ar||0r,|air||e3,|ai||eI,|ay||eI,|au||O,#:|al| |@l,#:|als| |@lz,|alk||Ok,|al|^|Ol, :|ab" +
			"le||eIb@l,|able||@b@l,|ang|+|eIndZ,^|a|^#|eI,|a||&, |be|^#|bI,|being||biIN, |both| |b@UT, |bus|#|bIz,|buil|" +
			"|bIl,|b||b, |ch|^|k,^e|ch||k,|ch||tS, s|ci|#|saI,|ci|a|S,|ci|o|S,|ci|en|S,|c|+|s,|ck||k,|com|%|kVm,|c||k,#:" +
			"|ded| |dId,.e|d| |d,#:^e|d| |t, |de|^#|dI, |do| |du, |does||dVz, |doing||duIN, |dow||daU,|du|a|dZu,|d||d,#:" +
			"|e| |,':^|e| |, :|e| |i,|ery||erI,#|ed| |d,#:|e|d |,|ev|er|ev,|e|n+r|e,|e|^%|i,|eri|#|iri,|eri||erI,#:|er|#" +
			"|3,|er|#|er,|er||3, |even||iven,#:|e|w|,t|ew||u,s|ew||u,r|ew||u,d|ew||u,l|ew||u,z|ew||u,n|ew||u,j|ew||u,th|" +
			"ew||u,ch|ew||u,sh|ew||u,|ew||ju,|e|o|i,#:s|es| |Iz,#:c|es| |Iz,#:g|es| |Iz,#:z|es| |Iz,#:x|es| |Iz,#:j|es| " +
			"|Iz,#:ch|es| |Iz,#:sh|es| |Iz,#:|e|s |,#:|ely| |li,#:|ement||ment,|eful||fUl,|ee||i,|earn||3n, |ear|^|3,|ea" +
			"d||ed,#:|ea| |i@,|ea|su|e,|ea||i,|eigh||eI,|ei||i, |eye||aI,|ey||i,|eu||ju,|e||e,|ful||fUl,|f||f,|giv||gIv," +
			" |g|i^|g,|ge|t|ge,su|gges||gdZes,|gg||g,#|gn|%|n,#|gn| |n, b#|g||g,|g|+|dZ,|great||greIt,#|gh||,|g||g, |hav" +
			"||h&v, |here||hir, |hour||aU3,|how||haU,|h|#|h,|h||, |iain| |I@n, |ing| |IN, |in||In, |i| |aI,|i'm||aIm,|in" +
			"|d|aIn,|ier||i3,#:r|ied||id,|ied| |aId,|ien||ien,|ie|t|aIe, :|i|%|aI,|i|%|i,|ie||i,|i|^+:#|I,|ir|#|aIr,|iz|" +
			"%|aIz,|is|%|aIz,|i|d%|aI,+^|i|^+|I,|i|t%|aI,#:^|i|^+|I,#|i|g|aI,|i|^+|aI,|ir||3,|igh||aI,|ild||aIld,|ique||" +
			"ik,^|i|^#|aI,|i||I,|j||dZ, |k|n|,|k||k,|lo|c#|l@U,l|l||,#:^|l|%|@l,|lead||lid,|l||l,|mov||muv,#|mm|#|m,|m||" +
			"m,e|ng|+|ndZ,|ng|r|Ng,|ng|#|Ng,|ngl|%|Ng@l,|ng||N,|nk||Nk, |now| |naU,#|ng| |Ng,|n||n,|of| |@v,|orough||3@U" +
			",#:|or| |3,#:|ors| |3z,|or||Or, |one||wVn,|ow||@U, |over||@Uv3,|ov||Vv,|o|^%|@U,|o|^en|@U,|o|^i#|@U,|ol|d|@" +
			"Ul,|ought||Ot,|ough||Vf, |ou||aU,h|ou|s#|aU,|ous||@s,|our||Or,|ould||Ud,^|ou|^l|V,|oup||up,|ou||aU,|oy||oI," +
			"|oing||@UIN,|oi||oI,|oor||Or,|ook||Uk,|ood||Ud,|oo||u,|o|e|@U,|o| |@U,|oa||@U, |only||@Unli, |once||wVns,|o" +
			"n't||@Unt,c|o|n|0,|o|ng|O, :^|o|n|V,i|on||@n,#:|on| |@n,#^|on||@n,|o|st |@U,|of|^|Of,|other||VD3,|oss| |Os," +
			"#:^|om||Vm,|o||0,|ph||f,|peop||piip,|pow||paU,|put| |pUt,|p||p,|quar||kwOr,|qu||kw,|q||k, |re|^#|ri,|r||r,|" +
			"sh||S,#|sion||Z@n,|some||sVm,#|sur|#|Z3,|sur|#|S3,#|su|#|Zu,#|ssu|#|Su,#|sed| |zd,#|s|#|z,|said||sed,^|sion" +
			"||S@n,|s|s|,.|s| |z,#:.e|s| |z,#:^##|s| |z,#:^#|s| |s,u|s| |s, :#|s| |z, |sch||sk,|s|c+|,#|sm||zm,#|sn|'|z@" +
			"n,|s||s, |the| |D@,|to| |tu,|that| |D&t, |this| |DIs, |they||DeI, |there||Der,|ther||D3,|their||Der, |than|" +
			" |D&n, |them| |Dem,|these| |Diz, |then||Den,|through||Tru,|those||D@Uz,|though| |D@U, |thus||DVs,|th||T,#:|" +
			"ted| |tId,s|ti|#n|tS,|ti|o|S,|ti|a|S,|tien||S@n,|tur|#|tS3,|tu|a|tSu, |two||tu,|t||t, |un|i|jun, |un||Vn, |" +
			"upon||@pOn,t|ur|#|Ur,s|ur|#|Ur,r|ur|#|Ur,d|ur|#|Ur,l|ur|#|Ur,z|ur|#|Ur,n|ur|#|Ur,j|ur|#|Ur,th|ur|#|Ur,ch|ur" +
			"|#|Ur,sh|ur|#|Ur,|ur|#|jUr,|ur||3,|u|^ |V,|u|^^|V,|uy||aI, g|u|#|,g|u|%|,g|u|#|w,#n|u||ju,t|u||u,s|u||u,r|u" +
			"||u,d|u||u,l|u||u,z|u||u,n|u||u,j|u||u,th|u||u,ch|u||u,sh|u||u,|u||ju,|view||vju,|v||v, |were||w3,|wa|s|w0," +
			"|wa|t|w0,|where||wer,|what||w0t,|whol||h@Ul,|who||hu,|wh||w,|war||wOr,|wor|^|w3,|wr||r,|w||w,|x||ks,|young|" +
			"|jVN, |you||ju, |yes||jes, |y||j,#:^|y| |i,#:^|y|i|i, :|y| |aI, :|y|#|aI, :|y|^+:#|I, :|y|^#|aI,|y||I,|z||z,";

		private static readonly Dictionary<u32string, u32string> FinnishAccentTab = new()
		{
			{"@U".ToUtf32(), "öy".ToUtf32()} /* OW |goat,shOW,nO|                               */,
			{"@".ToUtf32(), "ö".ToUtf32()} /* AX |About,commA,commOn|                         */,
			{"A".ToUtf32(), "aa".ToUtf32()} /* AA |stARt,fAther|                               */,
			{"&".ToUtf32(), "ä".ToUtf32()} /* AE |trAp,bAd|                                   */,
			{"V".ToUtf32(), "a".ToUtf32()} /* AH |strUt,bUd,lOve|                             */,
			{"Or".ToUtf32(), "oo".ToUtf32()} /* AR |wARp| */,
			{"O".ToUtf32(), "oo".ToUtf32()} /* AO |thOUGHt,lAW|                                */,
			{"aU".ToUtf32(), "au".ToUtf32()} /* AW |mOUth,nOW|                                  */,
			{"aI".ToUtf32(), "ai".ToUtf32()} /* AY |prIce,hIGH,trY|                             */,
			{"e@".ToUtf32(), "eö".ToUtf32()} /* EA |squARE,fAIR|                                */,
			{"eI".ToUtf32(), "ei".ToUtf32()} /* EY |fAce,dAy,stEAk|                             */,
			{"e".ToUtf32(), "e".ToUtf32()} /* EH |drEss,bEd|                                  */,
			{"3".ToUtf32(), "ör".ToUtf32()} /* ER |nURse,stIR,cOURage|                         */,
			{"ir".ToUtf32(), "iö".ToUtf32()} /* IA |nEAR,hERE,sErious|                          */,
			{"i@".ToUtf32(), "ia".ToUtf32()} /* EO |mEOw|                                       */,
			{"I".ToUtf32(), "i".ToUtf32()} /* IH |Intend,basIc,kIt,bId,hYmn|                  */,
			{"i".ToUtf32(), "i".ToUtf32()} /* IY |happY,radIation,glorIous,flEEce,sEA,machIne|*/,
			{"0r".ToUtf32(), "aa".ToUtf32()} /* ar |ARe */,
			{"0".ToUtf32(), "o".ToUtf32()} /* OH |lOt,Odd,wAsh|                               */,
			{"oI".ToUtf32(), "oi".ToUtf32()} /* OY |chOIce,bOY|                                 */,
			{"U@".ToUtf32(), "ue".ToUtf32()} /* UA |inflUence,sitUation,annUal,cURE,pOOR,jUry|  */,
			{"U".ToUtf32(), "u".ToUtf32()} /* UH |fOOt,gOOd,pUt,stimUlus,edUcate|             */,
			{"u".ToUtf32(), "uu".ToUtf32()} /* UW |gOOse,twO,blUE|                             */,
			{"Z@".ToUtf32(), "sö".ToUtf32()} /* viSIOn  */,
			{"b".ToUtf32(), "p".ToUtf32()} /* B |Back,BuBBle,joB|                             */,
			{"dZ".ToUtf32(), "ts".ToUtf32()} /* JH |JuDGE,aGe,solDIer|                          */,
			{"d".ToUtf32(), "d".ToUtf32()} /* D |Day,laDDer,oDD|                              */,
			{"D".ToUtf32(), "t".ToUtf32()} /* DH |THis,oTHer,smooTH|                          */,
			{"f".ToUtf32(), "v".ToUtf32()} /* F |Fat,coFFee,rouGH,PHysics|                    */,
			{"g".ToUtf32(), "k".ToUtf32()} /* G |Get,GiGGle,GHost|                            */,
			{"h".ToUtf32(), "h".ToUtf32()} /* HH |Hot,WHole,beHind|                           */,
			{"k&".ToUtf32(), "khä".ToUtf32()} /* CA |CAt,CAptain| */,
			{"k".ToUtf32(), "k".ToUtf32()} /* K |Key,CoCK,sCHool|                             */,
			{"l".ToUtf32(), "l".ToUtf32()} /* L |Light,vaLLey,feeL|                           */,
			{"m".ToUtf32(), "m".ToUtf32()} /* M |More,haMMer,suM|                             */,
			{"n".ToUtf32(), "n".ToUtf32()} /* N |Nice,KNow,fuNNy,suN|                         */,
			{"Ng".ToUtf32(), "ŋŋ".ToUtf32()} /* NG "coNGratulations" */,
			{"N".ToUtf32(), "ŋŋ".ToUtf32()} /* NG |riNG,loNG,thaNks,suNG|                      */,
			{"p".ToUtf32(), "p".ToUtf32()} /* P |Pen,coPy,haPPen|                             */,
			{"r".ToUtf32(), "r".ToUtf32()} /* R |Right,soRRy,aRRange|                         */,
			{"sw".ToUtf32(), "sv".ToUtf32()} /* SW |SWap| */,
			{"s".ToUtf32(), "s".ToUtf32()} /* S |Soon,CeaSe,SiSter|                           */,
			{"S".ToUtf32(), "s".ToUtf32()} /* SH |SHip,Sure,staTIon|                          */,
			{"tS".ToUtf32(), "ts".ToUtf32()} /* CH |CHurCH,maTCH,naTUre|                        */,
			{"ts".ToUtf32(), "ts".ToUtf32()} /* TS |TSai|                                       */,
			{"tu".ToUtf32(), "ty".ToUtf32()} /* TU |TUlip| */,
			{"t".ToUtf32(), "t".ToUtf32()} /* T |Tea,TighT,buTTon|                            */,
			{"T".ToUtf32(), "t".ToUtf32()} /* TH |THing,auTHor,paTH|                          */,
			{"v".ToUtf32(), "v".ToUtf32()} /* V |View,heaVy,moVe|                             */,
			{"w".ToUtf32(), "v".ToUtf32()} /* W |Wet,One,When,qUeen|                          */,
			{"j".ToUtf32(), "j".ToUtf32()} /* Y |Yet,Use,bEAuty|                              */,
			{"z".ToUtf32(), "s".ToUtf32()} /* Z |Zero,Zone,roSeS,buZZ|                        */,
			{"Z".ToUtf32(), "s".ToUtf32()} /* ZH |pleaSure,viSIon|                            */
		};

		private static readonly Dictionary<u32string, u32string> SymbolCanon = new()
		{
			{"0".ToUtf32(), "zero".ToUtf32()},
			{"1".ToUtf32(), "one".ToUtf32()},
			{"2".ToUtf32(), "two".ToUtf32()},
			{"3".ToUtf32(), "three".ToUtf32()},
			{"4".ToUtf32(), "four".ToUtf32()},
			{"5".ToUtf32(), "five".ToUtf32()},
			{"6".ToUtf32(), "six".ToUtf32()},
			{"7".ToUtf32(), "seven".ToUtf32()},
			{"8".ToUtf32(), "eight".ToUtf32()},
			{"9".ToUtf32(), "nine".ToUtf32()},
			{"c+".ToUtf32(), "ceeplus".ToUtf32()},
			{"+".ToUtf32(), "plus".ToUtf32()},
			{"/".ToUtf32(), "slash".ToUtf32()}
		};

		private static readonly Dictionary<u32string, u32string> PunctuationCanon = new()
		{
			{".".ToUtf32(), ">¯¯¯¯¯¯q<".ToUtf32()},
			{"!".ToUtf32(), ">¯¯¯¯¯¯q<".ToUtf32()},
			{"'".ToUtf32(), "q".ToUtf32()},
			{";".ToUtf32(), "¯¯¯¯|q".ToUtf32()},
			{"?".ToUtf32(), ">¯¯¯¯¯¯q<".ToUtf32()},
			{":".ToUtf32(), ">¯¯¯¯¯¯q<".ToUtf32()},
			{"-".ToUtf32(), "q".ToUtf32()},
			{",".ToUtf32(), "¯¯|q".ToUtf32()}
		};

		public Stream SynthesizeMessage(string message)
		{
			var phonemes = Phonemize(message);
			var audio = Vocalize(phonemes);
			throw new NotImplementedException();
		}

		private IEnumerable<byte> Vocalize(List<ProsodyElement> phonemes) => throw new NotImplementedException();

		private static List<ProsodyElement> Phonemize(string text)
		{
			var input = Canonize(text);

			var isVowel = new Func<u32char, bool>(c =>
				c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || c == 'y' || c == 'ä' || c == 'ö');
			var isAlphabet = new Func<u32char, bool>(c =>
				c >= 'a' && c <= 'z' || isVowel(c));
			//TODO: merge multiple definition in MatchesRulePattern
			var isConsonant = new Func<u32char, bool>(c =>
				isAlphabet(c) && !isVowel(c));
			var isEndPunctuation = new Func<u32char, bool>(c =>
				c == '>' || c == '¯' || c == 'q' || c == '"');

			throw new NotImplementedException();
		}

		private static void EnglishConvert(u32string text)
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
				var ruleIdx = row[1][0] >= 'a' && row[1][0] <= 'z' ? 1 + (row[1][0] - 'a') : 0;
				rules[ruleIdx] ??= new List<u32string[]>();
				rules[ruleIdx].Add(row);
			}

			var result = new u32string();
			for (var position = 1; position < text.Count;)
				position = ApplyConversionRules(text, position, rules, result);

			ChangeAccent(result, FinnishAccentTab);
		}

		private static int ApplyConversionRules(u32string text, int position, List<u32string[]>[] rules,
			u32string result)
		{
			var ruleMatched = false;
			var ruleIdx = text[position] >= 'a' && text[position] <= 'z' ? 1 + (text[position] - 'a') : 0;
			foreach (var rule in rules[ruleIdx])
			{
				u32string lPattern = rule[0], mPattern = rule[1], rPattern = rule[2];

				var lMatch = MatchesRulePattern(
					rPattern,
					text.GetEnumerator());
				var mMatch = text.Match(mPattern, position, mPattern.Count);
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
			var isConsonant = new Func<u32char, bool>(c => "bcdfghjklmnpqrstvwxyz".ToUtf32().Contains(c));

			//TODO: compare strings as strings instead of each char separately
			foreach (var patternChar in pattern)
				switch (patternChar)
				{
					case '#':
					{
						uint n = 0;
						for (; "aeiou".ToUtf32().Contains(textEnumerator.Current); n++)
							textEnumerator.MoveNext();
						if (n == 0)
							return false;
						break;
					}
					case ':':
						while (isConsonant(textEnumerator.Current)) textEnumerator.MoveNext();
						break;
					case '^':
						if (!isConsonant(textEnumerator.TryGetNext()))
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
						if (c >= 'a' && c <= 'z' || c == '\'')
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

		private static void ChangeAccent(u32string result, Dictionary<u32string, u32string> tab)
		{
			// At this point, the result string is pretty much an ASCII representation of IPA.
			// Now just touch up it a bit to convert it into typical Finnish pronunciation.
			LoggingService.LogAsync(new LogMessage(LogSeverity.Debug, nameof(SpeechService),
				$"Before: {result.ToUtf8()}"));

			for (var position = 0; position < result.Count; ++position)
				position = ApplyReplacementRules(result, tab, position);

			LoggingService.LogAsync(new LogMessage(LogSeverity.Debug, nameof(SpeechService),
				$"After: {result.ToUtf8()}"));
		}

		private static int ApplyReplacementRules(u32string result, Dictionary<u32string, u32string> rules, int position)
		{
			foreach (var (initial, replacement) in rules)
			{
				if (!result.Match(initial, position, initial.Count)) continue;

				result.RemoveRange(position, initial.Count);
				result.InsertRange(position, replacement);
				position += replacement.Count - 1;
				break;
			}

			return position;
		}

		private static u32string Canonize(string text)
		{
			var canonized = text.ToLowerInvariant().ToUtf32();

			for (var position = 0; position < canonized.Count;)
				position = ApplyReplacementRules(canonized, SymbolCanon, position);

			//pad input with leading and trailing space, so all patterns can match
			canonized.Insert(0, ' ');
			canonized.Add(' ');
			EnglishConvert(canonized);

			for (var position = 0; position < canonized.Count;)
				position = ApplyReplacementRules(canonized, PunctuationCanon, position);

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
			public u32char Record;
			public uint FramesCount;
			public float RelativePitch;
		}
	}
}