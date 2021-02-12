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
			var pcm = _speech.SynthesizeMessage(message);

			await Sp.WaitAsync();
			try
			{
				var voiceInfo = await _voice.JoinChannel(Context);
				await pcm.CopyToAsync(voiceInfo.AudioStream);
			}
			finally
			{
				Sp.Release();
			}
		}
	}
}