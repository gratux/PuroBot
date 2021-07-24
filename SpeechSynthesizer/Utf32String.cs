using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpeechSynthesizer
{
	public class Utf32String : IEnumerable<Utf32Char>
	{
		private Utf32String(string utf16String) => Chars = Utf32Char.FromUtf16(utf16String).ToList();

		public Utf32String() : this(string.Empty)
		{
		}

		public List<Utf32Char> Chars { get; }

		public Utf32Char this[int index]
		{
			get => Chars[index];
			set => Chars[index] = value;
		}

		public int Count => Chars.Count;
		public IEnumerator<Utf32Char> GetEnumerator() => Chars.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int IndexOfAny(int startIndex = 0, params Utf32Char[] chars) =>
			Chars.FindIndex(startIndex, chars.Contains);

		public Utf32Char? ElementAtOrDefault(int index) => Chars.ElementAtOrDefault(index);

		public bool Contains(Utf32Char? item) => item is not null && Chars.Contains(item);

		public void Clear() => Chars.Clear();

		public void Add(Utf32Char value) => Chars.Add(value);

		public void AddRange(Utf32String range) => Chars.AddRange(range.Chars);

		public override string ToString() => $"U\"{ToUtf16()}\"";

		public string ToUtf16() => string.Concat(Chars.Select(c => c.ToUtf16()));

		public static Utf32String FromUtf16(string utf16String) => new(utf16String);

		public static implicit operator Utf32String(string utf16String) => new(utf16String);

		public void Replace(int startIndex, int initialCount, Utf32String replacement) =>
			Chars.Replace(startIndex, initialCount, replacement.Chars);

		public void Insert(int i, Utf32Char c) => Chars.Insert(i, c);
	}
}