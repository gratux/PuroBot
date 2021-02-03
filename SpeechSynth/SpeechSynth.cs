using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SpeechSynth
{
	public class SynthBase
	{
		public async Task<Stream> Synthesize(string message)
		{
			var convert = new Process
			{
				StartInfo = new ProcessStartInfo("engsyn")
				{
					RedirectStandardInput = true,
					RedirectStandardOutput = true
				}
			};
			await convert.StandardInput.WriteAsync(message);
			convert.Start();
			return convert.StandardOutput.BaseStream;
		}
	}
}