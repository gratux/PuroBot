using System.Collections.Generic;

namespace PuroBot.SpeechSynth
{
	public class Utf32Char
	{
		private Utf32Char(int value) => Value = value;

		private int Value { get; }

		public string ToUtf16() => char.ConvertFromUtf32(Value);

		public static bool operator ==(Utf32Char lhs, Utf32Char rhs) => lhs is not null && lhs.Equals(rhs);

		public static bool operator !=(Utf32Char lhs, Utf32Char rhs) => !(lhs == rhs);

		public static implicit operator Utf32Char(char value) => new(value);
		public static implicit operator Utf32Char(int value) => new(value);

		public static int operator -(Utf32Char lhs, Utf32Char rhs) => lhs.Value - rhs.Value;
		public static int operator +(Utf32Char lhs, Utf32Char rhs) => lhs.Value + rhs.Value;

		private bool Equals(Utf32Char other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Utf32Char) obj);
		}

		public override int GetHashCode() => Value;

		public override string ToString() => $"U'{ToUtf16()}'";

		public static IEnumerable<Utf32Char> FromUtf16(string s)
		{
			for (var i = 0; i < s.Length; i++)
			{
				var unicodeCodePoint = char.ConvertToUtf32(s, i);
				if (unicodeCodePoint > 0xffff) i++;

				yield return new Utf32Char(unicodeCodePoint);
			}
		}

		
	}
}