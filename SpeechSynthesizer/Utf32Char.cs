using System.Collections.Generic;

namespace Bisqwit.SpeechSynthesizer
{
    public class Utf32Char
    {
        private Utf32Char(int value)
        {
            Value = value;
        }

        private int Value { get; }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Utf32Char) obj);
        }

        public static IEnumerable<Utf32Char> FromUtf16(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int unicodeCodePoint = char.ConvertToUtf32(s, i);
                if (unicodeCodePoint > 0xffff) i++;

                yield return new Utf32Char(unicodeCodePoint);
            }
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static int operator +(Utf32Char lhs, Utf32Char rhs)
        {
            return lhs.Value + rhs.Value;
        }

        public static bool operator ==(Utf32Char? lhs, Utf32Char? rhs)
        {
            return lhs is not null && lhs.Equals(rhs);
        }

        public static implicit operator Utf32Char(char value)
        {
            return new(value);
        }

        public static implicit operator Utf32Char(int value)
        {
            return new(value);
        }

        public static bool operator !=(Utf32Char? lhs, Utf32Char? rhs)
        {
            return !(lhs == rhs);
        }

        public static int operator -(Utf32Char lhs, Utf32Char rhs)
        {
            return lhs.Value - rhs.Value;
        }

        public override string ToString()
        {
            return $"U'{ToUtf16()}'";
        }

        public string ToUtf16()
        {
            return char.ConvertFromUtf32(Value);
        }

        private bool Equals(Utf32Char? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }
    }
}
