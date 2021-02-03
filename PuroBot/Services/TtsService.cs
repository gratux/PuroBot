using System.Diagnostics;
using System.IO;

namespace PuroBot.Services
{
	public static class TtsService
	{
		public static Stream Synthesize(string message)
		{
			using var engsyn = new Process
			{
				StartInfo = new ProcessStartInfo("engsyn")
				{
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					UseShellExecute = false
				}
			};
			
			engsyn.Start();
			engsyn.StandardInput.Write(message);

			return engsyn.StandardOutput.BaseStream;
		}
	}
}