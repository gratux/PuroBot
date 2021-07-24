using System.Collections.Generic;
using SpeechSynthesizer;
using u32char = System.Int32;
using u32string = System.Collections.Generic.List<int>;

// ReSharper disable BuiltInTypeReferenceStyle

namespace PuroBot.Discord.Services
{
	public class SpeechService
	{
		private readonly Synth _synth = new();

		public IEnumerable<byte> SynthesizeMessageAsync(string message) => _synth.SynthesizeText(message);
	}
}