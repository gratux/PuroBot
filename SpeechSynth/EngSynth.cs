using System;
using System.IO;

namespace SpeechSynth
{
	public class EngSynth : SynthBase
	{
		protected override MemoryStream Vocalize(string pattern)
		{
			throw new NotImplementedException();
		}

		protected override string ConvertToPattern(string message)
		{
			throw new NotImplementedException();
		}
	}
}