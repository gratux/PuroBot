namespace PuroBot.SpeechSynth
{
	public class PhonemeMapItem
	{
		public PhonemeMapItem(Utf32Char character, uint length, uint repeatedLength, uint surroundedLength)
		{
			Character = character;
			Length = length;
			RepeatedLength = repeatedLength;
			SurroundedLength = surroundedLength;
		}

		public Utf32Char Character { get; }
		public uint Length { get; }
		public uint RepeatedLength { get; }
		public uint SurroundedLength { get; }
	}
}