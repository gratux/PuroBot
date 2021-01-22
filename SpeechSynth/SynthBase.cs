using System.IO;

namespace SpeechSynth
{
	public abstract class SynthBase
	{
		protected abstract MemoryStream Vocalize(string pattern);
		protected abstract string ConvertToPattern(string message);

		public MemoryStream Synthesize(string message)
		{
			var pattern = ConvertToPattern(message);
			var pcmData = Vocalize(pattern);
			return pcmData;
		}
	}
}