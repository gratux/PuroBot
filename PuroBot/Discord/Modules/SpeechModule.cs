using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;
using PuroBot.Extensions;

namespace PuroBot.Discord.Modules
{
	[RequireUserPermission(GuildPermission.Administrator)]
	[RequireContext(ContextType.Guild)]
	public class SpeechModule : ModuleBase<SocketCommandContext>
	{
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
			using var channelAcquirer = await VoiceConnectionService.ChannelHandle.Create(_voice, Context);
			VoiceConnectionService.VoiceInfo? voiceInfo = channelAcquirer.VoiceInfo;
			if (voiceInfo is null) //failed to connect
				return;

			var pcm = _speech.SynthesizeMessageAsync(message).NormalizeAudio().ToArray();
			await voiceInfo.AudioStream.WriteAsync(pcm);
		}

		[Command("espeak")]
		public async Task ESpeakCommand([Remainder] string message)
		{
			var procInfo = new ProcessStartInfo
			{
				FileName = "/usr/bin/espeak-ng",
				ArgumentList = {"-w", "temp.wav", message},
				UseShellExecute = false
			};
			var proc = new Process {StartInfo = procInfo};
			proc.Start();
			await proc.WaitForExitAsync();

			procInfo = new ProcessStartInfo
			{
				FileName = "/usr/bin/ffmpeg",
				Arguments = "-i temp.wav -ac 2 -ar 48000 -f s16le -y temp.raw",
				UseShellExecute = false
			};
			proc = new Process {StartInfo = procInfo};
			proc.Start();
			await proc.WaitForExitAsync();

			using var channelAcquirer = await VoiceConnectionService.ChannelHandle.Create(_voice, Context);
			VoiceConnectionService.VoiceInfo? voiceInfo = channelAcquirer.VoiceInfo;
			if (voiceInfo is null) //failed to connect
				return;
			var pcm = await File.ReadAllBytesAsync("temp.raw");
			await voiceInfo.AudioStream.WriteAsync(pcm.NormalizeAudio().ToArray());
		}
	}
}