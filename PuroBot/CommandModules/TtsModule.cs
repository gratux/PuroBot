using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using PuroBot.Services;
using PuroBot.StaticServices;

namespace PuroBot.CommandModules
{
	public class TtsModule : ModuleBase<SocketCommandContext>
	{
		private readonly VoiceService _voice;
		
		private static readonly SemaphoreSlim Sp = new SemaphoreSlim(1);

		public TtsModule(VoiceService voice) => _voice = voice;
		
		[Command("tts")]
		public async Task TtsCommand([Remainder] string message)
		{
			await using var pcm = TtsService.Synthesize(message);
			
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