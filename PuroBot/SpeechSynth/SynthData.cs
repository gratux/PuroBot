using System.Collections.Generic;

namespace PuroBot.SpeechSynth
{
	public static class SynthData
	{
		public static readonly Utf32String[][] Patterns =
		{
			new Utf32String[] {".", "'s", "", "z"},
			new Utf32String[] {"#:.e", "'s", "", "z"},
			new Utf32String[] {"#", "'s", "", "z"},
			new Utf32String[] {"", "'", "", ""},
			new Utf32String[] {"", "a", " ", "@"},
			new Utf32String[] {" ", "are", " ", "0r"},
			new Utf32String[] {" ", "ar", "o", "@r"},
			new Utf32String[] {"", "ar", "#", "er"},
			new Utf32String[] {"^", "as", "#", "eIs"},
			new Utf32String[] {"", "a", "wa", "@"},
			new Utf32String[] {"", "aw", "", "O"},
			new Utf32String[] {" :", "any", "", "eni"},
			new Utf32String[] {"", "a", "^+#", "eI"},
			new Utf32String[] {"#:", "ally", "", "@li"},
			new Utf32String[] {" ", "al", "#", "@l"},
			new Utf32String[] {"", "again", "", "@gen"},
			new Utf32String[] {"#:", "ag", "e", "IdZ"},
			new Utf32String[] {"", "a", "^+:#", "&"},
			new Utf32String[] {" :", "a", "^+ ", "eI"},
			new Utf32String[] {"", "a", "^%", "eI"},
			new Utf32String[] {" ", "arr", "", "@r"},
			new Utf32String[] {"", "arr", "", "&r"},
			new Utf32String[] {" :", "ar", " ", "0r"},
			new Utf32String[] {"", "ar", " ", "3"},
			new Utf32String[] {"", "ar", "", "0r"},
			new Utf32String[] {"", "air", "", "e3"},
			new Utf32String[] {"", "ai", "", "eI"},
			new Utf32String[] {"", "ay", "", "eI"},
			new Utf32String[] {"", "au", "", "O"},
			new Utf32String[] {"#:", "al", " ", "@l"},
			new Utf32String[] {"#:", "als", " ", "@lz"},
			new Utf32String[] {"", "alk", "", "Ok"},
			new Utf32String[] {"", "al", "^", "Ol"},
			new Utf32String[] {" :", "able", "", "eIb@l"},
			new Utf32String[] {"", "able", "", "@b@l"},
			new Utf32String[] {"", "ang", "+", "eIndZ"},
			new Utf32String[] {"^", "a", "^#", "eI"},
			new Utf32String[] {"", "a", "", "&"},
			new Utf32String[] {" ", "be", "^#", "bI"},
			new Utf32String[] {"", "being", "", "biIN"},
			new Utf32String[] {" ", "both", " ", "b@UT"},
			new Utf32String[] {" ", "bus", "#", "bIz"},
			new Utf32String[] {"", "buil", "", "bIl"},
			new Utf32String[] {"", "b", "", "b"},
			new Utf32String[] {" ", "ch", "^", "k"},
			new Utf32String[] {"^e", "ch", "", "k"},
			new Utf32String[] {"", "ch", "", "tS"},
			new Utf32String[] {" s", "ci", "#", "saI"},
			new Utf32String[] {"", "ci", "a", "S"},
			new Utf32String[] {"", "ci", "o", "S"},
			new Utf32String[] {"", "ci", "en", "S"},
			new Utf32String[] {"", "c", "+", "s"},
			new Utf32String[] {"", "ck", "", "k"},
			new Utf32String[] {"", "com", "%", "kVm"},
			new Utf32String[] {"", "c", "", "k"},
			new Utf32String[] {"#:", "ded", " ", "dId"},
			new Utf32String[] {".e", "d", " ", "d"},
			new Utf32String[] {"#:^e", "d", " ", "t"},
			new Utf32String[] {" ", "de", "^#", "dI"},
			new Utf32String[] {" ", "do", " ", "du"},
			new Utf32String[] {" ", "does", "", "dVz"},
			new Utf32String[] {" ", "doing", "", "duIN"},
			new Utf32String[] {" ", "dow", "", "daU"},
			new Utf32String[] {"", "du", "a", "dZu"},
			new Utf32String[] {"", "d", "", "d"},
			new Utf32String[] {"#:", "e", " ", ""},
			new Utf32String[] {"':^", "e", " ", ""},
			new Utf32String[] {" :", "e", " ", "i"},
			new Utf32String[] {"", "ery", "", "erI"},
			new Utf32String[] {"#", "ed", " ", "d"},
			new Utf32String[] {"#:", "e", "d ", ""},
			new Utf32String[] {"", "ev", "er", "ev"},
			new Utf32String[] {"", "e", "n+r", "e"},
			new Utf32String[] {"", "e", "^%", "i"},
			new Utf32String[] {"", "eri", "#", "iri"},
			new Utf32String[] {"", "eri", "", "erI"},
			new Utf32String[] {"#:", "er", "#", "3"},
			new Utf32String[] {"", "er", "#", "er"},
			new Utf32String[] {"", "er", "", "3"},
			new Utf32String[] {" ", "even", "", "iven"},
			new Utf32String[] {"#:", "e", "w", ""},
			new Utf32String[] {"t", "ew", "", "u"},
			new Utf32String[] {"s", "ew", "", "u"},
			new Utf32String[] {"r", "ew", "", "u"},
			new Utf32String[] {"d", "ew", "", "u"},
			new Utf32String[] {"l", "ew", "", "u"},
			new Utf32String[] {"z", "ew", "", "u"},
			new Utf32String[] {"n", "ew", "", "u"},
			new Utf32String[] {"j", "ew", "", "u"},
			new Utf32String[] {"th", "ew", "", "u"},
			new Utf32String[] {"ch", "ew", "", "u"},
			new Utf32String[] {"sh", "ew", "", "u"},
			new Utf32String[] {"", "ew", "", "ju"},
			new Utf32String[] {"", "e", "o", "i"},
			new Utf32String[] {"#:s", "es", " ", "Iz"},
			new Utf32String[] {"#:c", "es", " ", "Iz"},
			new Utf32String[] {"#:g", "es", " ", "Iz"},
			new Utf32String[] {"#:z", "es", " ", "Iz"},
			new Utf32String[] {"#:x", "es", " ", "Iz"},
			new Utf32String[] {"#:j", "es", " ", "Iz"},
			new Utf32String[] {"#:ch", "es", " ", "Iz"},
			new Utf32String[] {"#:sh", "es", " ", "Iz"},
			new Utf32String[] {"#:", "e", "s ", ""},
			new Utf32String[] {"#:", "ely", " ", "li"},
			new Utf32String[] {"#:", "ement", "", "ment"},
			new Utf32String[] {"", "eful", "", "fUl"},
			new Utf32String[] {"", "ee", "", "i"},
			new Utf32String[] {"", "earn", "", "3n"},
			new Utf32String[] {" ", "ear", "^", "3"},
			new Utf32String[] {"", "ead", "", "ed"},
			new Utf32String[] {"#:", "ea", " ", "i@"},
			new Utf32String[] {"", "ea", "su", "e"},
			new Utf32String[] {"", "ea", "", "i"},
			new Utf32String[] {"", "eigh", "", "eI"},
			new Utf32String[] {"", "ei", "", "i"},
			new Utf32String[] {" ", "eye", "", "aI"},
			new Utf32String[] {"", "ey", "", "i"},
			new Utf32String[] {"", "eu", "", "ju"},
			new Utf32String[] {"", "e", "", "e"},
			new Utf32String[] {"", "ful", "", "fUl"},
			new Utf32String[] {"", "f", "", "f"},
			new Utf32String[] {"", "giv", "", "gIv"},
			new Utf32String[] {" ", "g", "i^", "g"},
			new Utf32String[] {"", "ge", "t", "ge"},
			new Utf32String[] {"su", "gges", "", "gdZes"},
			new Utf32String[] {"", "gg", "", "g"},
			new Utf32String[] {"#", "gn", "%", "n"},
			new Utf32String[] {"#", "gn", " ", "n"},
			new Utf32String[] {" b#", "g", "", "g"},
			new Utf32String[] {"", "g", "+", "dZ"},
			new Utf32String[] {"", "great", "", "greIt"},
			new Utf32String[] {"#", "gh", "", ""},
			new Utf32String[] {"", "g", "", "g"},
			new Utf32String[] {" ", "hav", "", "h&v"},
			new Utf32String[] {" ", "here", "", "hir"},
			new Utf32String[] {" ", "hour", "", "aU3"},
			new Utf32String[] {"", "how", "", "haU"},
			new Utf32String[] {"", "h", "#", "h"},
			new Utf32String[] {"", "h", "", ""},
			new Utf32String[] {" ", "iain", " ", "I@n"},
			new Utf32String[] {" ", "ing", " ", "IN"},
			new Utf32String[] {" ", "in", "", "In"},
			new Utf32String[] {" ", "i", " ", "aI"},
			new Utf32String[] {"", "i'm", "", "aIm"},
			new Utf32String[] {"", "in", "d", "aIn"},
			new Utf32String[] {"", "ier", "", "i3"},
			new Utf32String[] {"#:r", "ied", "", "id"},
			new Utf32String[] {"", "ied", " ", "aId"},
			new Utf32String[] {"", "ien", "", "ien"},
			new Utf32String[] {"", "ie", "t", "aIe"},
			new Utf32String[] {" :", "i", "%", "aI"},
			new Utf32String[] {"", "i", "%", "i"},
			new Utf32String[] {"", "ie", "", "i"},
			new Utf32String[] {"", "i", "^+:#", "I"},
			new Utf32String[] {"", "ir", "#", "aIr"},
			new Utf32String[] {"", "iz", "%", "aIz"},
			new Utf32String[] {"", "is", "%", "aIz"},
			new Utf32String[] {"", "i", "d%", "aI"},
			new Utf32String[] {"+^", "i", "^+", "I"},
			new Utf32String[] {"", "i", "t%", "aI"},
			new Utf32String[] {"#:^", "i", "^+", "I"},
			new Utf32String[] {"#", "i", "g", "aI"},
			new Utf32String[] {"", "i", "^+", "aI"},
			new Utf32String[] {"", "ir", "", "3"},
			new Utf32String[] {"", "igh", "", "aI"},
			new Utf32String[] {"", "ild", "", "aIld"},
			new Utf32String[] {"", "ique", "", "ik"},
			new Utf32String[] {"^", "i", "^#", "aI"},
			new Utf32String[] {"", "i", "", "I"},
			new Utf32String[] {"", "j", "", "dZ"},
			new Utf32String[] {" ", "k", "n", ""},
			new Utf32String[] {"", "k", "", "k"},
			new Utf32String[] {"", "lo", "c#", "l@U"},
			new Utf32String[] {"l", "l", "", ""},
			new Utf32String[] {"#:^", "l", "%", "@l"},
			new Utf32String[] {"", "lead", "", "lid"},
			new Utf32String[] {"", "l", "", "l"},
			new Utf32String[] {"", "mov", "", "muv"},
			new Utf32String[] {"#", "mm", "#", "m"},
			new Utf32String[] {"", "m", "", "m"},
			new Utf32String[] {"e", "ng", "+", "ndZ"},
			new Utf32String[] {"", "ng", "r", "Ng"},
			new Utf32String[] {"", "ng", "#", "Ng"},
			new Utf32String[] {"", "ngl", "%", "Ng@l"},
			new Utf32String[] {"", "ng", "", "N"},
			new Utf32String[] {"", "nk", "", "Nk"},
			new Utf32String[] {" ", "now", " ", "naU"},
			new Utf32String[] {"#", "ng", " ", "Ng"},
			new Utf32String[] {"", "n", "", "n"},
			new Utf32String[] {"", "of", " ", "@v"},
			new Utf32String[] {"", "orough", "", "3@U"},
			new Utf32String[] {"#:", "or", " ", "3"},
			new Utf32String[] {"#:", "ors", " ", "3z"},
			new Utf32String[] {"", "or", "", "Or"},
			new Utf32String[] {" ", "one", "", "wVn"},
			new Utf32String[] {"", "ow", "", "@U"},
			new Utf32String[] {" ", "over", "", "@Uv3"},
			new Utf32String[] {"", "ov", "", "Vv"},
			new Utf32String[] {"", "o", "^%", "@U"},
			new Utf32String[] {"", "o", "^en", "@U"},
			new Utf32String[] {"", "o", "^i#", "@U"},
			new Utf32String[] {"", "ol", "d", "@Ul"},
			new Utf32String[] {"", "ought", "", "Ot"},
			new Utf32String[] {"", "ough", "", "Vf"},
			new Utf32String[] {" ", "ou", "", "aU"},
			new Utf32String[] {"h", "ou", "s#", "aU"},
			new Utf32String[] {"", "ous", "", "@s"},
			new Utf32String[] {"", "our", "", "Or"},
			new Utf32String[] {"", "ould", "", "Ud"},
			new Utf32String[] {"^", "ou", "^l", "V"},
			new Utf32String[] {"", "oup", "", "up"},
			new Utf32String[] {"", "ou", "", "aU"},
			new Utf32String[] {"", "oy", "", "oI"},
			new Utf32String[] {"", "oing", "", "@UIN"},
			new Utf32String[] {"", "oi", "", "oI"},
			new Utf32String[] {"", "oor", "", "Or"},
			new Utf32String[] {"", "ook", "", "Uk"},
			new Utf32String[] {"", "ood", "", "Ud"},
			new Utf32String[] {"", "oo", "", "u"},
			new Utf32String[] {"", "o", "e", "@U"},
			new Utf32String[] {"", "o", " ", "@U"},
			new Utf32String[] {"", "oa", "", "@U"},
			new Utf32String[] {" ", "only", "", "@Unli"},
			new Utf32String[] {" ", "once", "", "wVns"},
			new Utf32String[] {"", "on't", "", "@Unt"},
			new Utf32String[] {"c", "o", "n", "0"},
			new Utf32String[] {"", "o", "ng", "O"},
			new Utf32String[] {" :^", "o", "n", "V"},
			new Utf32String[] {"i", "on", "", "@n"},
			new Utf32String[] {"#:", "on", " ", "@n"},
			new Utf32String[] {"#^", "on", "", "@n"},
			new Utf32String[] {"", "o", "st ", "@U"},
			new Utf32String[] {"", "of", "^", "Of"},
			new Utf32String[] {"", "other", "", "VD3"},
			new Utf32String[] {"", "oss", " ", "Os"},
			new Utf32String[] {"#:^", "om", "", "Vm"},
			new Utf32String[] {"", "o", "", "0"},
			new Utf32String[] {"", "ph", "", "f"},
			new Utf32String[] {"", "peop", "", "piip"},
			new Utf32String[] {"", "pow", "", "paU"},
			new Utf32String[] {"", "put", " ", "pUt"},
			new Utf32String[] {"", "p", "", "p"},
			new Utf32String[] {"", "quar", "", "kwOr"},
			new Utf32String[] {"", "qu", "", "kw"},
			new Utf32String[] {"", "q", "", "k"},
			new Utf32String[] {" ", "re", "^#", "ri"},
			new Utf32String[] {"", "r", "", "r"},
			new Utf32String[] {"", "sh", "", "S"},
			new Utf32String[] {"#", "sion", "", "Z@n"},
			new Utf32String[] {"", "some", "", "sVm"},
			new Utf32String[] {"#", "sur", "#", "Z3"},
			new Utf32String[] {"", "sur", "#", "S3"},
			new Utf32String[] {"#", "su", "#", "Zu"},
			new Utf32String[] {"#", "ssu", "#", "Su"},
			new Utf32String[] {"#", "sed", " ", "zd"},
			new Utf32String[] {"#", "s", "#", "z"},
			new Utf32String[] {"", "said", "", "sed"},
			new Utf32String[] {"^", "sion", "", "S@n"},
			new Utf32String[] {"", "s", "s", ""},
			new Utf32String[] {".", "s", " ", "z"},
			new Utf32String[] {"#:.e", "s", " ", "z"},
			new Utf32String[] {"#:^##", "s", " ", "z"},
			new Utf32String[] {"#:^#", "s", " ", "s"},
			new Utf32String[] {"u", "s", " ", "s"},
			new Utf32String[] {" :#", "s", " ", "z"},
			new Utf32String[] {" ", "sch", "", "sk"},
			new Utf32String[] {"", "s", "c+", ""},
			new Utf32String[] {"#", "sm", "", "zm"},
			new Utf32String[] {"#", "sn", "'", "z@n"},
			new Utf32String[] {"", "s", "", "s"},
			new Utf32String[] {" ", "the", " ", "D@"},
			new Utf32String[] {"", "to", " ", "tu"},
			new Utf32String[] {"", "that", " ", "D&t"},
			new Utf32String[] {" ", "this", " ", "DIs"},
			new Utf32String[] {" ", "they", "", "DeI"},
			new Utf32String[] {" ", "there", "", "Der"},
			new Utf32String[] {"", "ther", "", "D3"},
			new Utf32String[] {"", "their", "", "Der"},
			new Utf32String[] {" ", "than", " ", "D&n"},
			new Utf32String[] {" ", "them", " ", "Dem"},
			new Utf32String[] {"", "these", " ", "Diz"},
			new Utf32String[] {" ", "then", "", "Den"},
			new Utf32String[] {"", "through", "", "Tru"},
			new Utf32String[] {"", "those", "", "D@Uz"},
			new Utf32String[] {"", "though", " ", "D@U"},
			new Utf32String[] {" ", "thus", "", "DVs"},
			new Utf32String[] {"", "th", "", "T"},
			new Utf32String[] {"#:", "ted", " ", "tId"},
			new Utf32String[] {"s", "ti", "#n", "tS"},
			new Utf32String[] {"", "ti", "o", "S"},
			new Utf32String[] {"", "ti", "a", "S"},
			new Utf32String[] {"", "tien", "", "S@n"},
			new Utf32String[] {"", "tur", "#", "tS3"},
			new Utf32String[] {"", "tu", "a", "tSu"},
			new Utf32String[] {" ", "two", "", "tu"},
			new Utf32String[] {"", "t", "", "t"},
			new Utf32String[] {" ", "un", "i", "jun"},
			new Utf32String[] {" ", "un", "", "Vn"},
			new Utf32String[] {" ", "upon", "", "@pOn"},
			new Utf32String[] {"t", "ur", "#", "Ur"},
			new Utf32String[] {"s", "ur", "#", "Ur"},
			new Utf32String[] {"r", "ur", "#", "Ur"},
			new Utf32String[] {"d", "ur", "#", "Ur"},
			new Utf32String[] {"l", "ur", "#", "Ur"},
			new Utf32String[] {"z", "ur", "#", "Ur"},
			new Utf32String[] {"n", "ur", "#", "Ur"},
			new Utf32String[] {"j", "ur", "#", "Ur"},
			new Utf32String[] {"th", "ur", "#", "Ur"},
			new Utf32String[] {"ch", "ur", "#", "Ur"},
			new Utf32String[] {"sh", "ur", "#", "Ur"},
			new Utf32String[] {"", "ur", "#", "jUr"},
			new Utf32String[] {"", "ur", "", "3"},
			new Utf32String[] {"", "u", "^ ", "V"},
			new Utf32String[] {"", "u", "^^", "V"},
			new Utf32String[] {"", "uy", "", "aI"},
			new Utf32String[] {" g", "u", "#", ""},
			new Utf32String[] {"g", "u", "%", ""},
			new Utf32String[] {"g", "u", "#", "w"},
			new Utf32String[] {"#n", "u", "", "ju"},
			new Utf32String[] {"t", "u", "", "u"},
			new Utf32String[] {"s", "u", "", "u"},
			new Utf32String[] {"r", "u", "", "u"},
			new Utf32String[] {"d", "u", "", "u"},
			new Utf32String[] {"l", "u", "", "u"},
			new Utf32String[] {"z", "u", "", "u"},
			new Utf32String[] {"n", "u", "", "u"},
			new Utf32String[] {"j", "u", "", "u"},
			new Utf32String[] {"th", "u", "", "u"},
			new Utf32String[] {"ch", "u", "", "u"},
			new Utf32String[] {"sh", "u", "", "u"},
			new Utf32String[] {"", "u", "", "ju"},
			new Utf32String[] {"", "view", "", "vju"},
			new Utf32String[] {"", "v", "", "v"},
			new Utf32String[] {" ", "were", "", "w3"},
			new Utf32String[] {"", "wa", "s", "w0"},
			new Utf32String[] {"", "wa", "t", "w0"},
			new Utf32String[] {"", "where", "", "wer"},
			new Utf32String[] {"", "what", "", "w0t"},
			new Utf32String[] {"", "whol", "", "h@Ul"},
			new Utf32String[] {"", "who", "", "hu"},
			new Utf32String[] {"", "wh", "", "w"},
			new Utf32String[] {"", "war", "", "wOr"},
			new Utf32String[] {"", "wor", "^", "w3"},
			new Utf32String[] {"", "wr", "", "r"},
			new Utf32String[] {"", "w", "", "w"},
			new Utf32String[] {"", "x", "", "ks"},
			new Utf32String[] {"", "young", "", "jVN"},
			new Utf32String[] {" ", "you", "", "ju"},
			new Utf32String[] {" ", "yes", "", "jes"},
			new Utf32String[] {" ", "y", "", "j"},
			new Utf32String[] {"#:^", "y", " ", "i"},
			new Utf32String[] {"#:^", "y", "i", "i"},
			new Utf32String[] {" :", "y", " ", "aI"},
			new Utf32String[] {" :", "y", "#", "aI"},
			new Utf32String[] {" :", "y", "^+:#", "I"},
			new Utf32String[] {" :", "y", "^#", "aI"},
			new Utf32String[] {"", "y", "", "I"},
			new Utf32String[] {"", "z", "", "z"}
		};

		public static readonly Dictionary<Utf32Char, PhonemeMapItem[]> Maps = new()
		{
			{'m', new PhonemeMapItem[] {new('m', 8, 9, 0), new('M', 2, 0, 0)}},
			{'a', new PhonemeMapItem[] {new('a', 7, 9, 3)}},
			{'e', new PhonemeMapItem[] {new('e', 7, 9, 3)}},
			{'l', new PhonemeMapItem[] {new('l', 7, 9, 3)}},
			{'n', new PhonemeMapItem[] {new('n', 8, 9, 0), new('N', 2, 0, 0)}},
			{'u', new PhonemeMapItem[] {new('u', 7, 9, 3)}},
			{'y', new PhonemeMapItem[] {new('y', 7, 9, 3)}},
			{'s', new PhonemeMapItem[] {new('s', 7, 9, 3)}},
			{'ŋ', new PhonemeMapItem[] {new('ŋ', 8, 9, 0), new('Ŋ', 2, 0, 0)}},
			{'v', new PhonemeMapItem[] {new('v', 9, 9, 0)}},
			{'j', new PhonemeMapItem[] {new('j', 9, 9, 0)}},
			{'k', new PhonemeMapItem[] {new('-', 5, 9, 0), new('k', 4, 0, 0)}},
			{'i', new PhonemeMapItem[] {new('i', 7, 9, 3)}},
			{'o', new PhonemeMapItem[] {new('o', 7, 9, 3)}},
			{'p', new PhonemeMapItem[] {new('-', 5, 9, 0), new('p', 4, 0, 0)}},
			{'ä', new PhonemeMapItem[] {new('ä', 7, 9, 3)}},
			{'ö', new PhonemeMapItem[] {new('ö', 7, 9, 3)}},
			{'t', new PhonemeMapItem[] {new('-', 5, 9, 0), new('t', 4, 0, 0)}},
			{'r', new PhonemeMapItem[] {new('r', 9, 9, 0)}},
			{'h', new PhonemeMapItem[] {new('h', 9, 9, 0)}},
			{'d', new PhonemeMapItem[] {new('-', 5, 9, 0), new('d', 4, 0, 0)}},
			{'¯', new PhonemeMapItem[] {new('-', 7, 0, 0)}},
			{'q', new PhonemeMapItem[] {new('q', 1, 0, 0)}}
		};

		public static readonly Dictionary<Utf32String, Utf32String> FinnishAccentTab = new()
		{
			{"@U", "öy"} /* OW |goat,shOW,nO|                               */,
			{"@", "ö"} /* AX |About,commA,commOn|                         */,
			{"A", "aa"} /* AA |stARt,fAther|                               */,
			{"&", "ä"} /* AE |trAp,bAd|                                   */,
			{"V", "a"} /* AH |strUt,bUd,lOve|                             */,
			{"Or", "oo"} /* AR |wARp| */,
			{"O", "oo"} /* AO |thOUGHt,lAW|                                */,
			{"aU", "au"} /* AW |mOUth,nOW|                                  */,
			{"aI", "ai"} /* AY |prIce,hIGH,trY|                             */,
			{"e@", "eö"} /* EA |squARE,fAIR|                                */,
			{"eI", "ei"} /* EY |fAce,dAy,stEAk|                             */,
			{"e", "e"} /* EH |drEss,bEd|                                  */,
			{"3", "ör"} /* ER |nURse,stIR,cOURage|                         */,
			{"ir", "iö"} /* IA |nEAR,hERE,sErious|                          */,
			{"i@", "ia"} /* EO |mEOw|                                       */,
			{"I", "i"} /* IH |Intend,basIc,kIt,bId,hYmn|                  */,
			{"i", "i"} /* IY |happY,radIation,glorIous,flEEce,sEA,machIne|*/,
			{"0r", "aa"} /* ar |ARe */,
			{"0", "o"} /* OH |lOt,Odd,wAsh|                               */,
			{"oI", "oi"} /* OY |chOIce,bOY|                                 */,
			{"U@", "ue"} /* UA |inflUence,sitUation,annUal,cURE,pOOR,jUry|  */,
			{"U", "u"} /* UH |fOOt,gOOd,pUt,stimUlus,edUcate|             */,
			{"u", "uu"} /* UW |gOOse,twO,blUE|                             */,
			{"Z@", "sö"} /* viSIOn  */,
			{"b", "p"} /* B |Back,BuBBle,joB|                             */,
			{"dZ", "ts"} /* JH |JuDGE,aGe,solDIer|                          */,
			{"d", "d"} /* D |Day,laDDer,oDD|                              */,
			{"D", "t"} /* DH |THis,oTHer,smooTH|                          */,
			{"f", "v"} /* F |Fat,coFFee,rouGH,PHysics|                    */,
			{"g", "k"} /* G |Get,GiGGle,GHost|                            */,
			{"h", "h"} /* HH |Hot,WHole,beHind|                           */,
			{"k&", "khä"} /* CA |CAt,CAptain| */,
			{"k", "k"} /* K |Key,CoCK,sCHool|                             */,
			{"l", "l"} /* L |Light,vaLLey,feeL|                           */,
			{"m", "m"} /* M |More,haMMer,suM|                             */,
			{"n", "n"} /* N |Nice,KNow,fuNNy,suN|                         */,
			{"Ng", "ŋŋ"} /* NG "coNGratulations" */,
			{"N", "ŋŋ"} /* NG |riNG,loNG,thaNks,suNG|                      */,
			{"p", "p"} /* P |Pen,coPy,haPPen|                             */,
			{"r", "r"} /* R |Right,soRRy,aRRange|                         */,
			{"sw", "sv"} /* SW |SWap| */,
			{"s", "s"} /* S |Soon,CeaSe,SiSter|                           */,
			{"S", "s"} /* SH |SHip,Sure,staTIon|                          */,
			{"tS", "ts"} /* CH |CHurCH,maTCH,naTUre|                        */,
			{"ts", "ts"} /* TS |TSai|                                       */,
			{"tu", "ty"} /* TU |TUlip| */,
			{"t", "t"} /* T |Tea,TighT,buTTon|                            */,
			{"T", "t"} /* TH |THing,auTHor,paTH|                          */,
			{"v", "v"} /* V |View,heaVy,moVe|                             */,
			{"w", "v"} /* W |Wet,One,When,qUeen|                          */,
			{"j", "j"} /* Y |Yet,Use,bEAuty|                              */,
			{"z", "s"} /* Z |Zero,Zone,roSeS,buZZ|                        */,
			{"Z", "s"} /* ZH |pleaSure,viSIon|                            */
		};

		public static readonly Dictionary<Utf32String, Utf32String> EnglishAccentTab = new()
		{
			{"@U", "ou"} /* OW |goat,shOW,nO|                               */,
			{"@", "ö"} /* AX |About,commA,commOn|                         */,
			{"A", "aa"} /* AA |stARt,fAther|                               */,
			{"&", "ä"} /* AE |trAp,bAd|                                   */,
			{"V", "a"} /* AH |strUt,bUd,lOve|                             */,
			{"Or", "or"} /* AR |wARp| */,
			{"O", "oo"} /* AO |thOUGHt,lAW|                                */,
			{"aU", "au"} /* AW |mOUth,nOW|                                  */,
			{"aI", "ay"} /* AY |prIce,hIGH,trY|                             */,
			{"e@", "ea"} /* EA |squARE,fAIR|                                */,
			{"eI", "ey"} /* EY |fAce,dAy,stEAk|                             */,
			{"e", "e"} /* EH |drEss,bEd|                                  */,
			{"3", "ör"} /* ER |nURse,stIR,cOURage|                         */,
			{"ir", "ia"} /* IA |nEAR,hERE,sErious|                          */,
			{"i@", "ia"} /* EO |mEOw|                                       */,
			{"I", "i"} /* IH |Intend,basIc,kIt,bId,hYmn|                  */,
			{"i", "e"} /* IY |happY,radIation,glorIous,flEEce,sEA,machIne|*/,
			{"0r", "aa"} /* ar |ARe */,
			{"0", "o"} /* OH |lOt,Odd,wAsh|                               */,
			{"oI", "oy"} /* OY |chOIce,bOY|                                 */,
			{"U@", "ua"} /* UA |inflUence,sitUation,annUal,cURE,pOOR,jUry|  */,
			{"U", "u"} /* UH |fOOt,gOOd,pUt,stimUlus,edUcate|             */,
			{"u", "uu"} /* UW |gOOse,twO,blUE|                             */,
			{"Z@", "sö"} /* viSIOn  */,
			{"b", "p"} /* B |Back,BuBBle,joB|                             */,
			{"dZ", "j"} /* JH |JuDGE,aGe,solDIer|                          */,
			{"d", "d"} /* D |Day,laDDer,oDD|                              */,
			{"D", "d"} /* DH |THis,oTHer,smooTH|                          */,
			{"f", "v"} /* F |Fat,coFFee,rouGH,PHysics|                    */,
			{"g", "k"} /* G |Get,GiGGle,GHost|                            */,
			{"h", "h"} /* HH |Hot,WHole,beHind|                           */,
			{"k&", "khä"} /* CA |CAt,CAptain| */,
			{"k", "k"} /* K |Key,CoCK,sCHool|                             */,
			{"l", "l"} /* L |Light,vaLLey,feeL|                           */,
			{"m", "m"} /* M |More,haMMer,suM|                             */,
			{"n", "n"} /* N |Nice,KNow,fuNNy,suN|                         */,
			{"Ng", "ŋŋ"} /* NG "coNGratulations" */,
			{"N", "ŋŋ"} /* NG |riNG,loNG,thaNks,suNG|                      */,
			{"p", "p"} /* P |Pen,coPy,haPPen|                             */,
			{"r", "r"} /* R |Right,soRRy,aRRange|                         */,
			{"sw", "sv"} /* SW |SWap| */,
			{"s", "s"} /* S |Soon,CeaSe,SiSter|                           */,
			{"S", "s"} /* SH |SHip,Sure,staTIon|                          */,
			{"tS", "ts"} /* CH |CHurCH,maTCH,naTUre|                        */,
			{"ts", "ts"} /* TS |TSai|                                       */,
			{"tu", "tu"} /* TU |TUlip| */,
			{"t", "t"} /* T |Tea,TighT,buTTon|                            */,
			{"T", "t"} /* TH |THing,auTHor,paTH|                          */,
			{"v", "v"} /* V |View,heaVy,moVe|                             */,
			{"w", "v"} /* W |Wet,One,When,qUeen|                          */,
			{"j", "y"} /* Y |Yet,Use,bEAuty|                              */,
			{"z", "s"} /* Z |Zero,Zone,roSeS,buZZ|                        */,
			{"Z", "s"} /* ZH |pleaSure,viSIon|                            */
		};

		public static readonly Dictionary<Utf32String, Utf32String> SymbolCanon = new()
		{
			{"0", "zero"},
			{"1", "one"},
			{"2", "two"},
			{"3", "three"},
			{"4", "four"},
			{"5", "five"},
			{"6", "six"},
			{"7", "seven"},
			{"8", "eight"},
			{"9", "nine"},
			{"c+", "seeplus"},
			{"+", "plus"},
			{"/", "slash"}
		};

		public static readonly Dictionary<Utf32String, Utf32String> PunctuationCanon = new()
		{
			{".", ">¯¯¯¯¯¯q<"},
			{"!", ">¯¯¯¯¯¯q<"},
			{"'", "q"},
			{";", "¯¯¯¯|q"},
			{"?", ">¯¯¯¯¯¯q<"},
			{":", ">¯¯¯¯¯¯q<"},
			{"-", "q"},
			{",", "¯¯|q"}
		};

		public static readonly Dictionary<Utf32Char, Record> Records = new()
		{
			{
				'a', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(.1 / 673, new[]
					{
						-3.0756, 6.13225, -9.8759, 13.4059, 9 / -.557, 17.4074, -16.741, 14.2528, 4 / -.387, 5.78307,
						-1.7385, -1.1577, 2.21281, -1.5154, -.7941, 4.48439, -8.6406, 12.6773, -15.578, 16.928, -16.609,
						14.635, -11.505, 7.85801,
						-4.2802, 5 / 3.122, -.06644, -.21514, -.65011, 1.98091, -3.5922, 4.96125, -5.8751, 6.09519,
						-5.57, 4.56488, .7358 - 4, 1.91885, -.75213, -.12763, .700502, -.86701, .775098, -.50139,
						.8749 / 3, -.14123, 2 / 50.94, -3.3 / 89
					}),
					new Frame(.3 / 688, new[]
					{
						-2.3452, 3.46519, -4.5415, 19 / 3.79, -4.8651, 4.16283, -3.0387, 89.1 / 47, -1.0474, .461726,
						-.44479, .447668, -.42375, .1552, .646875, -1.4021, 2.32686, -3.0314, 3.15497, -2.8762, 2.4737,
						-1.7906, 1.23765, -.77125,
						.629912, -.79805, .864391, -87.0 / 68, 85.9 / 56, -1.4868, 1.19119, -.5665, -.16817, .655168,
						-1.0673, 7 / 5.723, -1.1893, 6 / 4.957, 9 / -8.69, .934526, -.81225, .853362, -.93096, .780533,
						-.58959, .418935, 8 / -41.9, 3 / 763.3
					})
				})
			},
			{
				'e', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(2 / 979.0, new[]
					{
						-1.4722, 1.95384, -2.3325, 2.23476, -2.1611, 1.66578, 9 / -7.37, 6 / 4.753, -1.2878, 1.60391,
						-15.8 / 9, 1.32406, -1.3018, .805713, -.58958, .454371, -.22361, 8 / 42.99, -.13739, -.06761,
						2.06 / 53, -.41272, .331551, -.20553,
						.376382, -.17777, .328064, -.32093, .346064, -.5357, .692398, -.87689, 1.19381, 5 / .72 - 8,
						7 / 5.844, -.98924, .741983, 4 - 4.613, .487421, -.60752, .696716, -.56935, .53913, -.37944,
						.158724, -.09196, -.15293, .3309 / 6
					})
				})
			},
			{
				'i', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(389e-6, new[]
					{
						-2.1853, 3.54645, -4.7987, 6.08678, -7.4337, 8.62016, -9.4488, 8 / .7901, -10.232, 9.97198,
						-9.4982, 7 / .825, -7.6404, 6.33999, -5.1773, 3.88024, -2.8952, 2.05013, -1.6431, 1.31278,
						.4406 - 2, 88 / 49.3, -2.4697, 3.29694,
						-4.0415, 4.9552, -5.519, 6.03142, -6.1734, 6.08352, -5.6244, 5.11247, -4.2345, 3.58192, -2.6332,
						1.90681, .9164 - 2, .376851, .025371, -.26113, .43932, -.56511, .666731, -.63017, .462952,
						-.36185, .120752, -.08849
					})
				})
			},
			{
				'o', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(.1 / 817, new[]
					{
						-2.8334, 5.23896, -7.5823, 8.97126, -9.4831, 8.7479, -50.9 / 7, 5.58178, 7 / 6e3 - 4, 2.86901,
						-2.3263, 2.11154, -2.4635, 3.03454, -3.6268, 3.94126, -3.4785, 2.53351, -7.49 / 6, 27 / 96.6,
						-.15397, .887097, 7 / -3.65, 2.85419,
						-2.9474, 2.22463, -.84297, -.71842, 1.83963, -2.4016, 2.23192, .5289 - 2, 25.1 / 94, 1.03957,
						-2.353, 3.33935, -3.8803, 3.71826, -2.8979, 1.82912, -.84961, .311892, -.13796, .171686,
						-.20739, .186232, -.10914, 8.3 / 223
					})
				})
			},
			{
				'u', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(.1 / 815, new[]
					{
						-2.7013, 5.42646, -8.8033, 12.3787, -15.779, 17.8556, -18.413, 16.9032, -13.796, 9.43483,
						-5.0286, 6 / 5.879, 1.51929, -2.5558, 1.87837, -.27 / 46, -2.1384, 4.19718, -5.0962, 4.97397,
						-3.5219, 1.44781, .998909, -3.0574,
						4.65709, -5.3054, 5.51325, -5.3616, 5.21287, 3 / -.557, 5.72285, -6.4907, 6.96743, .7845 - 8,
						6.77, 3 - 8.858, 4.35576, -2.6331, .787661, .716609, -1.7236, 2.17338, -2.0362, 1.72114,
						-1.1542, .698283, -.2684, .055049
					})
				})
			},
			{
				'y', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(6 / 67e3, new[]
					{
						-3.0469, 5.5079, -7.3726, 8.16721, 5 / 7.1 - 9, 8.12794, -8.2457, 8.98131, -10.01, 10.7066,
						-10.534, 9.23308, -7.2465, 4.99159, -2.9656, 1.40979, -.38546, .038267, -.31467, .951765,
						-1.5384, 1.6928, -15.2 / 9, 1.74853,
						2 - 4.376, 3.69594, -5.2809, 6.56069, -6.8487, 6.07025, -4.7254, 3.32983, -2.2993, 1.53554,
						-.77753, -.94 / 39, .935207, -1.7749, 2.45026, -2.8401, 2.90847, -2.7361, 2.55843, -2.2528,
						1.83483, -1.2242, 58 / 94.5, -.1963
					}),
					new Frame(7 / 11e3, new[]
					{
						-2.3924, 3.64745, -4.3307, 4.39804, 34 / -8.3, 3.25503, -2.3574, 1.75951, -1.1007, .550043,
						.115899, -.91329, 76.1 / 47, -2.4907, 2.75497, -2.2471, 1.12488, 59 / 513.0, -.80721, .975268,
						-.93266, .628787, -.73908, 8.57 / 8,
						-1.7508, 76 / 29.9, -2.8699, 2.83355, -2.4186, 1.6778, -.96607, .400757, -.17424, .081751,
						.334253, 7 / -8.29, 1.42706, -1.7088, 39 / 23.2, -1.4573, .926668, -.29602, -.04112, .251376,
						-.23851, .207323, -.392 / 9, -.06063
					})
				})
			},
			{
				'ä', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(.4 / 811, new[]
					{
						-2.6607, 4.16229, -5.3294, 6.01665, -6.3883, 6.13845, -5.1924, 3.95241, -2.3822, .645071,
						2 / 1.582, -3.1817, 4.64234, -5.3215, 5.04397, -4.1794, 2.81948, -.63939, .9153 - 3, 4.85924,
						-7.1595, 8.71716, -9.5544, 9.4789,
						-8.5385, 6.81002, -4.5895, 2.28552, -.05804, -1.7835, 3.00525, -3.712, 3.84861, -3.379, 2.40862,
						-92.0 / 81, -.17826, 1.4302, -2.3718, 2.95128, -3.2238, 3.30288, 1 - 4.134, 2.73964, -2.1799,
						4.364 / 3, -6.38 / 9, .183897
					}),
					new Frame(.8 / 467, new[]
					{
						-2.3921, 3.80438, -5.2711, 6.43621, -7.3198, 7.62828, -7.4906, 7.196, -6.4919, 5.74504, -4.9881,
						3.74797, 2 / -.815, 1.16791, .10674, -1.2781, 2.2616, -2.5644, 2.60718, -2.5189, 8.992 - 7,
						2 / -1.24, 13 / 9.92, -.94367,
						.571493, -.1298, 13 / -77.0, .607039, -.94411, .807218, -4.14 / 7, .261066, .184999, -.64309,
						1.09063, .6704 - 2, 7 / 4.621, -1.5266, 1.43981, -1.4093, 1.36984, 3 - 4.156, .972935, -.8092,
						.591514, -.33155, .090427, -.02728
					})
				})
			},
			{
				'ö', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(.5 / 751, new[]
					{
						-2.6628, 4.50584, -6.6181, 9.05284, -11.806, 14.3185, -16.575, 18.7312, 69 / -3.4, 21.191,
						4 / -.187, 20.8685, -19.871, 18.3709, -16.591, 14.4861, 4 / -.33, 9.83151, -7.8638, 6.29521,
						-4.9776, 3.69673, -2.8843, 2.42854,
						-1.9431, 1.40703, 5 / -6.01, .400622, 82 / 739.0, .1054 - 1, 157.0 / 91, -2.471, 3.14418,
						-3.725, 26.09 / 6, -4.8615, 5.23395, -5.371, 5.17639, -4.8156, 4.33627, -3.6674, 2.9649,
						-2.2689, 1.74369, 9 / -7.72, .58536, -.19318
					}),
					new Frame(.1 / 79, new[]
					{
						-2.1651, 3.11789, -3.9982, 90.3 / 19, -5.6037, 5.82241, -5.3717, 4.75547, -3.8815, 3.10536,
						-2.2491, 1.20039, -.63533, .43448, -.60152, 56.7 / 55, -1.6538, 2.49999, -3.3836, 4.06491,
						-4.5457, 4.51749, -4.2559, 3.7119,
						-2.9409, 2.06762, -3.23 / 3, .363345, 5 / 86.56, -.32802, .40402, -.39414, .231045, 6 / 48.37,
						2 - 2 / .89, .289419, -.29333, 7 / 20.4, -.39904, .174232, .194606, -.36364, .516199, -.55489,
						.650853, -.58815, .369023, -.15537
					})
				})
			},
			{
				'l', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(135e-7, new[]
					{
						-2.8667, 5 - .83 / 6, 2 - 8.296, 6.86253, -7.0243, 6.87889, -7.3349, 8.60232, -9.9499, 10.589,
						-10.405, 9.30542, -7.4484, 5.35398, -4.0968, 3.76606, -3.3893, 2.93649, -2.0213, .7532, .544805,
						-1.4852, 2.05136, -2.6565,
						3.08847, -3.0401, 27 / 9.17, -3.2529, 3.99904, -5.0841, 6.12224, 1 - 8.018, 7.11725,
						3 / 9.7 - 7, 6.22432, -5.7495, 5.40096, -5.1806, 19 / 3.69, 6 - 4 / .37, 4.03813, -3.0226,
						2.16851, -1.4754, 1.02303, -.7074, .453281, -.21644
					})
				})
			},
			{
				'v', new Record(RecordModes.PlayInSequence, 1.0, new[]
				{
					new Frame(.9 / 727, new[]
					{
						-.6475, .895097, -.49507, 1.04554, -.73018, .656294, -.43997, .656349, -.51219, .616832,
						-.59456, .576566, -.38377, .347036, -.43534, .319422, -.45709, .258793, -.50503, .235445,
						-.44788, .219917, -.38769, 5291e-7,
						-.25146, .043262, 8 / -32.9, -.07853, .8671 - 1, -.41 / 21, -8.3 / 78, .017544, -3.9 / 37,
						.156887, 7 / -59.4, .044117, -6.2 / 77, 26 / 557.0, 7307e-6, .027723, 2 / -53.3, 2.14 / 17,
						.042671, .080142, .755 / 54, 7.09 / 69, 37 / 955.0, 3.15 / 59
					}),
					new Frame(.00204, new[]
					{
						-.47269, .883427, -.39786, 1.14491, -.65688, .747957, -.37602, .821296, -.39003, .631003,
						-.31432, .561629, -.29738, .344391, -.23949, .248084, -.22554, .140133, -.20182, -.04952,
						-.09965, 6 / -895.0, -.08985, -.1287,
						.9691 - 1, -.11495, .8143 - 1, 5 / -29.7, -.12424, -.13961, -.17663, -.817 / 9, -.17993,
						-.07238, -.20892, -.04115, 9 / -43.3, 5.6 / 331, -.18693, -.03223, -.05263, 9 / -895.0,
						-.35 / 63, 3 / -683.0, .521 / 84, 4 / 95.1, 2 / 47.84, 7 / 944.0
					}),
					new Frame(2 / 698.0, new[]
					{
						-.44571, .993593, -.3577, 1.34363, -.59479, 1.01352, -.39456, 1.04985, -.35472, .844917,
						-.25724, .682021, -.11532, .461431, -.06523, .351209, -.3 / 781, .205478, 5 / -329.0, .076189,
						9 / 444.7, -.03963, 2 / -89.9, -8.5 / 66,
						-.01505, -.21688, -.07772, -.24395, .9561 - 1, -.34938, -.71 / 53, -.45187, .171 / 8, -.40962,
						4 / -99.9, -.38146, -.03829, -.32808, -.07219, -.22996, 3 / 313.2, -.18382, -.03867, 5 / -64.4,
						-.33 / 31, -.59 / 7, 2 / -86.1, 2 / -64.9
					}),
					new Frame(2 / 802.0, new[]
					{
						-.40004, 1.00544, -.29271, 1.20601, -.38725, .897696, -.17342, .915912, -.07703, .857522,
						-.323 / 7, .637206, .130735, .437649, .733 / 22, .352018, .082602, 1.583 / 7, .991 / 59,
						.128035, 8.7 / 781, 8 / -65e2, 9 / -97.3, -.11096,
						-.10157, -.18137, -.22043, -.25326, -.18083, -.32835, -.23056, -.3256, -.29339, -.25717,
						-.30692, -.25399, -.31668, -.22532, -.19929, -.32137, -.1402, -.26685, -.1068, -.21604, -.02402,
						-.09482, -.496 / 7, 7 / -477.0
					}),
					new Frame(.18 / 79, new[]
					{
						-.45905, 1.02573, -.27825, 1.30331, -.40999, .965109, -.16485, .975009, -.04881, .876956,
						2.83 / 39, .687165, .186656, .653473, .196572, .548092, .200638, .528383, 3 / 50.26, .502353,
						-3.1 / 86, .362546, -.10353, .205271,
						-.05847, 2.51 / 29, -.14404, 21 / 767.0, -.13967, -.02084, -.20805, -.0913, -.18743, -.15179,
						-.20707, -.21701, -.19486, -.799 / 3, -4.1 / 29, -.33124, -.06527, -.23819, -.05476, -.22529,
						-.0413, -.06615, -.04754, -.719 / 7
					})
				})
			},
			{
				'j', new Record(RecordModes.PlayInSequence, 1.0, new[]
				{
					new Frame(1.0 / 829, new[]
					{
						-.9329, 1.34835, -.99885, 9.819 / 8, -.98692, 1.25368, -.83294, .955671, 2 - 2.74, .735051,
						-.86139, .522761, -.64664, .241166, -.63375, .223395, -.50647, 7.1 / 789, .5669 - 1, 3.03 / 86,
						-5.27 / 9, .169174, -.47546, 6 / 31.7,
						-.40888, .335166, -.38162, .290337, -.23608, .383813, 9 / -53.5, .434459, -.05225, .316159,
						-.19 / 87, .317835, -.08897, .292921, -.17158, .244017, -.17015, .193835, -.552 / 7, 2 / 59.91,
						2 / -50.2, 3 / 1691.0, -.06971, .508 / 39
					}),
					new Frame(.15 / 79, new[]
					{
						-.97911, 1.40929, -1.1562, 1.40453, -1.1176, 1.2862, -.91432, .97109, -.81332, .727181, -.88536,
						.533273, -.60703, .290972, -.67669, .377926, -.69115, .326997, -.64493, 3.743 / 9, -.69097,
						.446731, -.54854, .382845,
						-.43855, .376437, -.27463, .204716, 5.3 / 253, 9.7 / 523, .220779, -.07168, .273583, -.85 / 32,
						.115337, .085419, -.185 / 7, .118848, -.21004, 2 / 7.308, 17 / -57.0, .342201, -.34515, .303226,
						-.31341, .210274, -.17328, .071893
					}),
					new Frame(.8 / 313, new[]
					{
						-1.1028, 1.54069, 7 - 8.342, 92.8 / 63, -1.1098, 1.08562, -.6204, .580421, -.33485, 8.26 / 27,
						-.25526, .8704 - 1, .073076, -.41294, .169271, -.35041, .088394, -.27523, -.0746, 3 / -563.0,
						-.35992, 2 / 7.889, -.50672, .565362,
						-.59567, .445664, -.33343, .266448, -.25387, .258073, -.26625, .247939, -.13208, .169864,
						.066957, -.01948, .078461, .637 / 55, -.07057, .119764, -.0571, .083025, 5 / 406.2, 9 / 461.9,
						6 / 2017.0, 5.5 / 967, -.04317, 9 / 526.9
					})
				})
			},
			{
				'm', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(7 / 54e5, new[]
					{
						3 / -.945, 4.88992, -4.7182, 3.22114, -2.2612, 2.32769, -2.5441, 1.98927, -.57403, -.41747,
						-.40044, 2.12053, -8.15 / 3, 1.70855, -.69205, .868405, -1.6767, 2.33279, 7 / -3.41, .577909,
						.792832, -1.0844, .885758, -1.1115,
						79.7 / 53, -1.1074, .164415, .647364, -.87061, .299868, .2505, -.03826, -.53278, .674518,
						-.43946, .500538, -.83726, .918205, -.73663, .427638, -.13495, 11 / 80.4, -.39004, .597553,
						-.55886, .404423, -.28713, .124834
					}),
					new Frame(2 / 19e5, new[]
					{
						-3.2808, 5.26018, -5.3441, 3.9408, 2 - 4.788, 2.14926, -1.4476, .571725, .33607, -.80798,
						.124254, 1.36021, 8 / 9.3 - 3, 1.44485, -.57838, .493165, -.53641, .51628, -.5151, .244816,
						.299205, -.52052, .297518, -.56868,
						65 / 56.7, -.95302, .296193, 66 / 895.0, -.17225, 7 / 20.96, -.45839, .250922, 2.12 / 37,
						-.40993, .683488, -.47836, 49 / 774.0, -1.1 / 79, .295587, -.36506, .235387, -.45622, .984345,
						-1.3265, 1.34604, -1.0459, .59075, -.18385
					})
				})
			},
			{
				'n', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(985e-9, new[]
					{
						-2.6444, 3.54402, -3.3668, 2.88497, -2.6671, 1.75735, -.82885, .575718, -.15586, -9.9 / 23,
						.196563, .933084, -1.1051, -.685 / 3, 4.227 - 3, -1.5782, 1.54527, -.87143, 8 / -62.1, .559218,
						-.20456, .407399, .9119 - 2, .827405,
						.2393 / 3, -.44061, .317653, -.09774, -.47639, 1.33341, .6584 - 2, .499209, -.25503, .620054,
						-.70964, .517867, -.49755, .667767, -.47977, .240432, -.16315, -.971 / 9, .467213, -.72427,
						.576593, -.26201, .054931, .026447
					})
				})
			},
			{
				'ŋ', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(85 / 6e7, new[]
					{
						-2.8337, 3.89908, .61 / 7 - 4, 3.68805, .2137 - 4, 3.42694, -2.4361, 1.67903, -.84371,
						-.244 / 7, .063339, .587172, -.91564, .356869, .5371 / 3, -.2328, .277927, -.04243, -.46116,
						.419245, .346429, -.90767, .485358, 9 / -238.0,
						.352507, -.44468, 9 / 95.27, .064285, 4 / -35.3, .407565, -.61166, .048786, .614779, -.55219,
						.214708, .062464, -.28394, .381587, -.25755, 8.53 / 41, -.45057, .467101, 7 / -56.5, -.17626,
						.213264, -.1162, .083797, -.04073
					})
				})
			},
			{
				'r', new Record(RecordModes.Trill, 0.7, new[]
				{
					new Frame(.5 / 568, new[]
					{
						-1.6581, 1.47599, 7 / 8.9 - 2, .801652, -.49712, .405337, -.25194, .081484, .05476, -.19527,
						.253281, -.36338, .443025, -.31547, .079332, -.04978, 6 / 68.99, -.55 / 96, -.26409, .435705,
						-.53198, .454273, -4.19 / 9, .546337,
						-.43583, .243277, 6 / 90.1, -.26746, .249108, -9.2 / 67, 2 / 9.723, -.2582, .256444, -.18498,
						6.9 / 58, -.17292, .073941, .113725, -.10673, .136745, -.13855, .133534, -.18396, .210636,
						-.18642, .4228 / 9, .042504, 8 / -929.0
					}),
					new Frame(.7 / 837, new[]
					{
						-1.4456, 1.75389, .2086 - 2, 96 / 55.3, -1.5799, 1.49007, 7 - 8.422, 1.40126, 3 / -2.51, .99806,
						-.8846, .710935, -.41194, 9.38 / 47, 8 / -357.0, -.13295, .148547, -.14632, -.46 / 11, .197128,
						17 / -49.0, .393738, 9 / -14.6, .748186,
						-.85231, .892035, -.86068, .704621, -.57174, .448498, -.35547, .5311 / 7, .132598, -.33589,
						.344439, -.42553, .439514, -.40533, .369633, -.33658, .324646, -.2467, 9.77 / 74, -.65 / 21,
						-.04551, .108697, -.09663, 6 / 92.81
					})
				})
			},
			{
				'h', new Record(RecordModes.ChooseRandomly, 0.8, new[]
				{
					new Frame(.3 / 326, new[]
					{
						-2.3061, 3.88204, -5.1106, 6.10614, -6.4723, 6.5779, -6.3288, 5.96964, -5.3612, 4.68547,
						.3 / 67 - 4, 3.55241, -3.3794, 3.50295, -3.7882, 4.19627, -4.5491, 4.90824, .4 / 71 - 5,
						5.19628, -5.0859, 5.04539, -4.6606, 4.40548,
						-3.9627, 3.68062, -3.302, 3.13745, -2.9582, 2.84891, -2.7233, 2.64826, -2.5197, 2.42155,
						6 - 8.297, 2.13512, -1.9437, 1.7161, -1.3474, .973625, -.55785, .344364, -.08077, .0874 / 5,
						.043989, 369 / 8e6, .151 / 86, 4 / 949.5
					}),
					new Frame(.5 / 318, new[]
					{
						-2.0228, 3.06492, -3.6295, 336.0 / 83, -3.8739, 3.81194, 1 - 4.459, 3.23292, -2.6221, 3 / 1.403,
						-1.5143, 1.16498, -.72622, .518561, -.21214, .180833, -.0807, .262755, -.33091, .612002,
						.5266 - 1, .488424, -.29714, 1.061 / 3,
						-.15872, .233748, 7 / 98.99, 29 / -98.0, .816579, -1.08, 1.47501, 3 / .67 - 6, 1.55277, -1.3157,
						1.20528, -.93076, 57.8 / 81, -.39883, .128446, .222904, -.32916, .551483, -.61888, .630743,
						-.51511, .384857, -.19696, 5 / 78.38
					}),
					new Frame(.7 / 575, new[]
					{
						-2.0407, 3.03252, -3.7201, 4.32004, -4.2901, 4.25101, -3.8697, 3.59146, -2.9079, 152.0 / 63,
						-1.7158, .4367 * 3, -.80688, .593301, -.30763, .434744, -.47325, 5 / 5.966, -1.0088, 1.43759,
						.5858 - 2, 9 / 6.224, 5 / -4.28, 1.11667,
						-.72407, .578636, -.17068, -.481 / 9, .548415, -.76321, 1.11841, -1.1101, 6.707 / 6, -.84135,
						.610499, -.17642, -.15649, .580787, -.77743, 7.559 / 7, .8881 - 2, 1.20729, -1.0868, .983729,
						-.72307, .476638, -.20188, .8989 / 9
					})
				})
			},
			{
				's', new Record(RecordModes.ChooseRandomly, 0.6, new[]
				{
					new Frame(.08655, new[]
					{
						-.7649, 1.97483, 83 / -98.0, 2.22238, -.39569, 2.11838, .092921, 2.08693, .299204, 2.05626,
						.421377, 1.95985, .460823, 1.83253, .395346, 1.79008, .208773, 1.8452, .12295, 1.80929, .166766,
						1.6017, .326999, 9.731 / 7,
						.41325, 1.35398, .316937, 1.40627, .230771, 7 / 5.538, .271141, 67 / 63.9, .216408, .906089,
						.150757, .722434, .212324, 35.8 / 89, .329995, .115311, .35407, .9529 - 1, .30735, -1.3 / 17,
						.181739, -.01144, .068921, .843 / 29
					})
				})
			},
			{
				'M', new Record(RecordModes.PlayInSequence, 1.0, new[]
				{
					new Frame(2 / 19e5, new[]
					{
						-3.2808, 5.26018, -5.3441, 3.9408, -.788 - 2, 2.14926, -1.4476, .571725, .33607, -.80798,
						.124254, 1.36021, 8 / 9.3 - 3, 1.44485, -.57838, .493165, -.53641, .51628, -.5151, .244816,
						.299205, -.52052, .297518, -.56868,
						65 / 56.7, -.95302, .296193, 66 / 895.0, -.17225, 7 / 20.96, -.45839, .250922, 2.12 / 37,
						-.40993, .683488, -.47836, 49 / 774.0, -1.1 / 79, .295587, -.36506, .235387, -.45622, .984345,
						-1.3265, 1.34604, -1.0459, .59075, -.18385
					}),
					new Frame(3e-6, new[]
					{
						-2.7154, 4.04813, -4.7594, 5.10211, -5.3123, 4.78003, 7 / 32.0 - 4, 2.91794, -2.1311, 79 / 51.4,
						7 - 8.473, 1.64784, .2693 - 2, 1.47415, 7 - 8.137, .809236, -.30744, 6.48 / 31, -.33972,
						.386435, -.55853, .662506, -.64647, .452329,
						-.27033, .339936, -.35205, .532409, -.86321, 1.10576, 9 - 9 / .86, 1.69911, -1.5747, 93 / 80.9,
						-.72475, .467083, -.13569, -.19372, .205166, 9 / -235.0, 8 / -46.9, .382655, -.48822, .507608,
						-.46589, .436249, -.31666, .109615
					}),
					new Frame(726e-7, new[]
					{
						-2.7055, 5.27142, -8.3589, 3 / .2628, -14.36, 634.0 / 39, -51.4 / 3, 16.89, -15.815, 14.2345,
						-12.698, 11.3152, -10.413, 9.75213, -9.3064, 9.0326, -8.6146, 8.24944, -7.6631, 7.1692, -6.6442,
						6.09073, -5.5277, 4.78707,
						1 - 4.948, 9.076 / 3, -2.1012, 1.34406, -.63724, .122305, .2261 / 6, -.1053, .077584, 9 / -55.9,
						.323691, -.52452, 26.3 / 33, -1.0205, 67 / 58.2, .8153 - 2, 73 / 64.6, -.984, .842721, -.63258,
						.432972, -.24376, .101924, -.242 / 7
					})
				})
			},
			{
				'N', new Record(RecordModes.PlayInSequence, 1.0, new[]
				{
					new Frame(258e-7, new[]
					{
						-2.1681, 3.24958, -4.4536, 5.3615, -6.068, 6.45926, .52 / 7 - 7, 7.4441, -7.6526, 7.86745,
						-7.7654, 7.14427, -6.3148, 28 / 5.45, -4.1248, 3.0606, -2.2956, 1.65141, -1.0021, .422221,
						5 / 95.91, 5 / 7.6 - 1, .387713, -.37107,
						.084365, 3 / 6.73, -.85296, 1.29656, -1.7239, 1.95945, -2.0913, 2.24937, -2.3503, 2.47087,
						-2.4715, 2.4679, -2.2851, 2.03754, 2 / 9.1 - 2, 8.591 / 6, 3 - 4.126, .882324, -.63924, .469911,
						-.28758, .162497, -.0709, -.1 / 17
					})
				})
			},
			{
				'Ŋ', new Record(RecordModes.PlayInSequence, 1.0, new[]
				{
					new Frame(621e-8, new[]
					{
						-2.6269, 4.03076, 3 - 8.447, 41.71 / 6, -8.1051, 8.0495, -7.7638, 7.4754, -6.1168, 4.33254,
						-2.9225, 1.71812, -.3394, -.65321, 26 / 44.1, -.62885, .948322, -.71267, .236821, -.23513,
						.61573, -.57267, .24622, -.29579,
						.224489, .522953, -1.0585, .960048, -.8125, .833816, -.58008, -5.7 / 82, .253858, -.26537,
						.689895, -.88859, .637248, -.27152, 1.009 / 7, .133189, -.62081, .649804, -.42836, .341612,
						-6.2 / 21, .139149, .059256, -.06249
					}),
					new Frame(7 / 58e4, new[]
					{
						.2794 - 2, 1.4326, -.87499, .615333, 8 - 8.738, .29433, -.26602, .523019, -.50489, .547933,
						-.54677, .646015, -.69802, .378624, -.2396, 99 / 676.0, -.29733, .523173, -.39027, .325181,
						-.3119, .304514, -.34249, 8 / 33.13,
						-.29508, .464843, -.26811, .164567, .83 / 501, -.01195, -.10614, -.07134, 7 / 84.95, -.16742,
						.194242, -.19472, .255676, -.5 / 47, -.07854, .10545, 7 / -74.7, -.46 / 55, .048004, -.541 / 9,
						.129 / 8, -.01001, .086801, -.07495
					}),
					new Frame(.7 / 743, new[]
					{
						.497 - 2, .902683, -.30876, .359744, -.38603, .179944, -.20872, .769055, .53 / 9 - 1, .426104,
						-.15576, 84 / 241.0, -.60886, .350831, 6 / -93.7, .214902, 3 / -8.62, .281743, -.39163, .548153,
						-.58715, .217571, 2 / -26.2, .153568,
						-.22094, .190773, -.24597, .395003, -.40099, .206022, -.17916, .234022, -.18999, 1.64 / 23,
						.101141, -.12761, .07292, .1135 / 8, -.07526, .148291, -.11464, 6 / 70.05, 1.31 / 49, 5 / 480.3,
						-.05234, .106428, -.613 / 9, 7.7 / 718
					}),
					new Frame(.4 / 943, new[]
					{
						-2.0955, 2.52516, -2.6486, 2.60716, -2.3884, 1.82981, -1.3921, 67 / 43.3, -1.3994, .995632,
						-8.84 / 9, .89539, -.74728, .439074, -.31711, .357272, .7084 - 1, .425844, -.59158, .647597,
						-.82686, .770321, -.65957, .526562,
						-.55396, .635593, -.50301, .643509, -.82708, .709423, -.57817, .436207, -.35218, .22774,
						.025963, -9.1 / 99, .256134, -.34377, .354339, -.36901, .338933, -.30361, .360488, -.32173,
						.257568, -.16719, .144225, -.11455
					})
				})
			},
			{
				'k', new Record(RecordModes.PlayInSequence, 0.7, new[]
				{
					new Frame(5 / 29e2, new[]
					{
						-.25849, .611723, -.24763, 4.95 / 53, -.26475, .388188, -.45517, .243741, -.56888, .048597,
						-.59354, .462965, -.05008, .386215, 7 / 820.6, 13 / 59.1, 8 / -76.7, .420037, -.02581, .415649,
						9.9 / 716, .121133, -.14905, 2 / -91.7,
						-.465 / 8, .4826 / 3, -.71 / 92, .071025, -.14727, .107635, -.02797, .261547, .9586 - 1,
						.108921, -.14549, 56 / 827.0, -.06209, 56 / 631.0, -.04404, .6047 / 6, -.12404, .947 / 11,
						.039175, .095456, 3.3 / 163, .104949, -.06682, .043599
					}),
					new Frame(.62 / 67, new[]
					{
						-.5765, 86.8 / 83, -.84647, .568173, -.82526, .66107, -.90441, .840275, -1.003, .735946,
						-1.1266, .706031, -.73953, .875951, -.45982, .786789, -.4285, .569538, -.3667, .552809,
						9 / -50.7, .37581, -.10338, .12022,
						-.35287, 6 / 27.37, -.23242, .266914, -.10347, .195181, -.15918, 4.05 / 88, -.22342, .130995,
						-.33724, .321468, 3 / -11.1, 64 / 283.0, -5.9 / 44, .176362, 5 / -46.1, .204493, -8.7 / 71,
						.256418, -.1599, .214098, -.676 / 9, .049621
					}),
					new Frame(.3 / 821, new[]
					{
						-1.6179, 2.04832, -1.9326, 71.3 / 47, -1.4434, 1.29345, .5693 - 2, 13.06 / 9, -1.5168,
						9.936 / 7, -1.5084, 1.19738, -7.87 / 7, 1.15347, -1.1128, 61 / 49.6, -86.0 / 79, 67 / 58.7,
						-1.0434, 1.03936, -.95235, .904597, -.8484, .826726,
						-.82207, .731457, -.58544, .501126, -.36329, 10.6 / 37, -.28178, .258587, -.39804, .370787,
						-.36736, .247414, -.20078, .115685, -.14823, .120843, -7.3 / 48, .141658, -6.9 / 73, 3 / 32.63,
						3 / -73.4, .109533, -.04688, .03882
					})
				})
			},
			{
				'p', new Record(RecordModes.PlayInSequence, 0.9, new[]
				{
					new Frame(2 / 56e6, new[]
					{
						-.26321, -.96 / 29, -.33925, -.41 / 94, -.0696, -.04211, -.16382, .081204, -.04202, 6 / 26.57,
						-2.9 / 61, .156277, -.20147, -.02664, -.88 / 87, .058844, -.1042, .034266, -5.9 / 48, .049459,
						-.09603, 22 / 815.0, -.04769, .151127,
						-.16155, .9658 / 7, .9199 - 1, 6.13 / 69, -.05539, .083376, -7.7 / 67, .077893, -.05995,
						69 / 974.0, 3 / -856.0, 3 / 128.6, 6 / -72.1, 4 / 52.75, -.09139, 5 / -909.0, .166 / 41,
						5.3 / 62, -.09075, .087532, -.14996, .033713, -.89 / 57, 9 / -395.0
					}),
					new Frame(.3 / 982, new[]
					{
						-1.0702, 66 / 57.1, -8.15 / 8, .773536, -.48633, .419291, -.38126, .574453, -.6295, .762945,
						-.70768, .65284, -.46365, 22 / 56.7, -.17264, 7.19 / 24, -.36135, .535804, -.61726, .51547,
						-.36819, .300551, -.17475, .192803,
						-.18658, .274643, -.20754, .239828, 4 / -25.3, .12126, -.0639, .069868, -.49 / 13, .111197,
						-.09232, .718 / 13, .029036, -.05659, .5084 / 3, -.13121, .138592, -.10001, .9157 / 9,
						-.22 / 73, 2.34 / 63, -.07697, 1.6 / -23, 8 / 5071.0
					})
				})
			},
			{
				't', new Record(RecordModes.PlayInSequence, 0.7, new[]
				{
					new Frame(881e-8, new[]
					{
						-.89126, .701657, -.7126, .612496, -.32309, .330047, -.03858, .239781, 7 / 84.71, 8 / 723.5,
						-.16935, .127532, 8 - 8.149, .208273, -.20558, .321708, 2 / -6.01, .227606, -.3061, .217452,
						-.32516, .156449, -.01377, .9875 - 1,
						.911 / 73, -.12492, 2 / -49.0, 2 / -83.6, -.06228, 2.83 / 74, -.07437, .099885, -7.9 / 89,
						.5603 / 8, 8 - 8.13, .11951, -.12811, 7 / 53.16, -.37 / 13, 6 / 413.9, 4 / -961.0, .032664,
						.16 / 413, -.03202, 465e-5, .363 / 17, .048597, 5.8 / 659
					}),
					new Frame(.02631, new[]
					{
						.602 - 2, 69.5 / 39, 3 - 4.682, 72.8 / 41, -1.5846, 1.51559, -1.1939, 1.33671, -.9272, .929477,
						-.69681, .69203, -.56617, .573276, -.52362, .623209, -.52694, .530791, -.38437, .37537, -.36918,
						.37411, 8 / -42.7, 6 / 42.23,
						-.04199, .2434 / 3, -.7 / 78, .063089, -.05239, .095619, 2 / -60.4, .07735, 8 / 71e3, 9 / 90.5,
						-401e-5, .093362, -.29 / 49, .081034, 4.15 / 63, 7.9 / 384, -2.9 / 47, 13 / 56.9, .8221 - 1,
						4.43 / 23, -.12069, .186275, -.06471, .040343
					}),
					new Frame(.7 / 466, new[]
					{
						-1.7313, 2.49357, -2.7112, 2.8339, -2.5662, 2.31174, -1.9007, 1.60754, -.99784, .644546, -.3459,
						.197805, -.06224, 7 / -681.0, -.83 / 22, .139429, -.31205, .407598, -.43229, .398769, -.874 / 3,
						.073401, -.02565, -.09598,
						.051925, -.18572, 73 / 708.0, -.16496, .109376, -.14614, -.19 / 58, 2.77 / 76, -.18032, .275291,
						-.36962, .417901, -.36321, .352191, -.33741, .382667, -.30694, .296188, -.17607, 8 / 59.83,
						-.7 / 305, 7.9 / 568, -.86 / 97, .4743 / 8
					})
				})
			},
			{
				'd', new Record(RecordModes.PlayInSequence, 1.0, new[]
				{
					new Frame(7 / 548.0, new[]
					{
						-1.307, 1.28644, -1.5592, 1.28694, -.88068, 1.16025, .9004 - 2, .877746, -.51092, .305495,
						-.32585, .259843, -.52827, .844296, -.57442, .572513, -.71168, .663275, -.57026, .660771,
						-.52679, .361551, -.24283, 52 / 217.0,
						-.18574, -.241 / 6, .251742, -.33744, .566191, -.57126, .366128, 2 / -4.59, .337784, 2 / -8.1,
						.311524, 2 / -26.3, .6679 / 6, 9 / -52.1, .161195, -.38259, .408347, -.34056, .351641,
						2 / .54 - 4, .252183, -.23916, .121665, -.07156
					}),
					new Frame(.26 / 87, new[]
					{
						-1.9612, 2.67465, -3.3428, 3.35979, -3.1352, 2.66914, -61.0 / 32, 61 / 36.4, -1.3039, 3.143 / 3,
						-1.2591, 1.09495, -4.31 / 3, 1.68517, 9 - 5 / .48, 6 / 4.155, -1.2239, .929525, -.90448,
						.616796, -.72946, .784038, -.70655, .776582,
						-.68948, .779152, -.87377, .686287, -.62323, .570786, -.51535, .519798, -.47601, .57461, -.4857,
						.47585, -.49652, .508143, -.65295, .83184, -.80903, .722045, -.50449, .298488, 2 / -29.3,
						2 / -19.4, .093609, -.0536
					}),
					new Frame(.9 / 263, new[]
					{
						-2.2087, 3.31825, -4.0701, 3.96729, -3.6029, 2.83823, -1.8897, 1.39099, -.71116, .307475,
						7 / -52.3, -.395, 2 / 5.466, -.45059, .472017, -.2967, .426199, -.20614, 7 / -403.0, .085793,
						-.47467, 5 / 9.309, -.726, .875983,
						-.6817, .695754, -.44552, 23.8 / 95, -.08832, -.27165, .331501, -.43206, .436196, -9.8 / 85,
						.011487, .283297, -3.05 / 9, .12467, 2.77 / 62, -4.9 / 23, .147372, -.00899, .08001, -.07588,
						.133649, -.14493, .5585 / 8, -.535 / 7
					})
				})
			},
			{
				'q', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(399e-8, new[]
					{
						-1.0053, .981036, .5953 - 1, 8 / -961.0, 8 / -69.7, -.11517, -.03382, -.16259, .7819 / 5,
						-.15481, 2 / 49.34, 7 / -79.2, .090498, 6 / -425.0, -3.6 / 83, .101639, -5.6 / 63, .334682,
						-.21832, .204355, -.206 / 3, .028362, 1 - 5 / 5.1, 2 / -214.0,
						.069921, .8771 - 1, .149598, .8452 - 1, .7672 / 9, -.11521, .723 / 71, -.06599, .019285,
						-2.6 / 51, -.05196, 9 / 712.8, .024002, -.08534, 6 / 99.49, -.185 / 7, 9 / 649.7, 63 / 971.0,
						9 / 385.3, -.04514, .229425, -.22968, .227591, -.13816
					}),
					new Frame(5 / 29e2, new[]
					{
						-.25849, .611723, -.24763, 4.95 / 53, -.26475, .388188, -.45517, .243741, -.56888, .048597,
						-.59354, .462965, -.05008, .386215, 7 / 820.6, 13 / 59.1, 8 / -76.7, .420037, -.02581, .415649,
						9.9 / 716, .121133, -.14905, 2 / -91.7,
						-.465 / 8, .4826 / 3, -.71 / 92, .071025, -.14727, .107635, -.02797, .261547, .9586 - 1,
						.108921, -.14549, 56 / 827.0, -.06209, 56 / 631.0, -.04404, .6047 / 6, -.12404, .947 / 11,
						.039175, .095456, 3.3 / 163, .104949, -.06682, .043599
					})
				})
			},
			{
				'-', new Record(RecordModes.ChooseRandomly, 1.0, new[]
				{
					new Frame(115e-9, new[]
					{
						.9753 - 2, 1.06833, -.84778, .56465, -.27841, -.03447, -.67 / 48, .163857, -.20012, .428255,
						-.59245, 21 / 50.6, -.34458, .270356, -.17169, .090675, -.05959, .125303, .048079, -.25814,
						.9773 / 8, -.26702, .174393, 5.31 / 41,
						-.33924, .382773, -.3126, .222216, -.064 / 3, -.26069, .297382, -.1804, .140001, 8.1 / 598,
						-.15812, .150066, -.13765, .128137, -.977 / 8, .21833, -.10268, .199576, -.06301, 3 / 4274.0,
						-.91 / 92, 2 / -90.4, 6 / -221.0, -.05152
					}),
					new Frame(71 / 2e9, new[]
					{
						-.72894, 1.786 / 3, -.34094, 8.74 / 33, 7 / -85.5, -.07249, -.982 / 9, .229312, -.24488,
						.319863, -.33001, .099976, -.02195, -1.1 / 79, 5.42 / 85, -.21884, 11 / 92.0, -.06903, .148414,
						-.20422, .035743, -.19842, 96 / 541.0, 11 / -92.0,
						.046707, 3 / -62.0, 4 / 909.3, .054134, 1.6 / -69, -.05576, .5111 / 9, 2227e-6, 3 / 76.76,
						.9338 - 1, 62 / 999.0, -9.1 / 62, .187464, 5 / -36.2, .068574, .121431, -.61 / 13, .095215,
						.091302, -.08489, .121855, .9438 - 1, .06808, -.08139
					})
				})
			}
		};
	}
}