namespace Bisqwit.SpeechSynthesizer
{
    public class ProsodyElement
    {
        public ProsodyElement(Utf32Char recordChar, uint framesCount, double relativePitch)
        {
            RecordChar = recordChar;
            FramesCount = framesCount;
            RelativePitch = relativePitch;
        }

        public Utf32Char RecordChar { get; }

        public uint FramesCount { get; }

        public double RelativePitch { get; }
    }
}
