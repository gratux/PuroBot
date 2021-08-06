using System.Collections.Generic;
using Bisqwit.SpeechSynthesizer;

namespace PuroBot.Discord.Services
{
    public class SpeechService
    {
        public IEnumerable<byte> SynthesizeMessageAsync(string message)
        {
            return _synth.SynthesizeText(message);
        }

        private readonly Synth _synth = new();
    }
}
