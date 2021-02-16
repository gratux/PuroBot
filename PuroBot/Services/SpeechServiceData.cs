using System.Collections.Generic;
using PuroBot.Extensions;
using u32char = System.Int32;
using u32string = System.Collections.Generic.List<int>;

// ReSharper disable BuiltInTypeReferenceStyle

namespace PuroBot.Services
{
	public partial class SpeechService
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

		private static readonly Dictionary<u32char, PhonemeMapItem[]> Maps = new()
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

		private static readonly u32string Vowels = @"aeiouyäüö".ToUtf32();
		private static readonly u32string Consonants = @"bcdfghjklmnpqrstvwxz".ToUtf32();
		private static readonly u32string Alphabet = @"abcdefghijklmnopqrstuvwxyzäüö".ToUtf32();

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
	}
}