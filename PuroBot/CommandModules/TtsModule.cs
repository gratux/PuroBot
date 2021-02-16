using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using PuroBot.Services;

namespace PuroBot.CommandModules
{
	public class TtsModule : ModuleBase<SocketCommandContext>
	{
		private static readonly SemaphoreSlim Sp = new(1);
		private readonly SpeechService _speech;
		private readonly VoiceService _voice;

		public TtsModule(VoiceService voice, SpeechService speech)
		{
			_voice = voice;
			_speech = speech;
		}

		[Command("tts")]
		public async Task TtsCommand([Remainder] string message)
		{
			var pcm = await _speech.SynthesizeMessageAsync(message);

			await Sp.WaitAsync();
			try
			{
				var voiceInfo = await _voice.JoinChannel(Context);
				await voiceInfo.AudioStream.WriteAsync(pcm);
			}
			finally
			{
				Sp.Release();
			}
		}
	}
}