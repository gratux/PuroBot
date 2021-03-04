using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;
using PuroBot.Extensions;

namespace PuroBot.Discord.CommandModules
{
	[RequireUserPermission(GuildPermission.Administrator)]
	[RequireContext(ContextType.Guild)]
	public class SpeechModule : ModuleBase<SocketCommandContext>
	{
		private static readonly SemaphoreSlim Sp = new(1);
		private readonly SpeechService _speech;
		private readonly VoiceConnectionService _voice;

		public SpeechModule(VoiceConnectionService voice, SpeechService speech)
		{
			_voice = voice;
			_speech = speech;
		}

		[Command("speak")]
		[Alias("tts")]
		public async Task SpeechCommand([Remainder] string message)
		{
			using var channelAcquirer = await VoiceConnectionService.ChannelAcquirer.Create(_voice, Context);
			var voiceInfo = channelAcquirer.VoiceInfo;
			if (voiceInfo is null) //failed to connect
				return;

			var pcm = _speech.SynthesizeMessageAsync(message).NormalizeAudio().ToArray();
			await voiceInfo.AudioStream.WriteAsync(pcm);
		}
	}
}