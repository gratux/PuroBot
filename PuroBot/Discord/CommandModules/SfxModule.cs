using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;
using PuroBot.Extensions;

namespace PuroBot.Discord.CommandModules
{
	[Group("sfx")]
	[Summary("play a sound file")]
	[RequireUserPermission(GuildPermission.Administrator)]
	[RequireContext(ContextType.Guild)]
	public class SfxModule : ModuleBase<SocketCommandContext>
	{
		private const string BaseAudioPath = "Resources/SpeakAudio/";
		private const string AudioExt = "pcm";
		private static readonly SemaphoreSlim Sp = new(1);
		private readonly VoiceConnectionService _voice;

		public SfxModule(VoiceConnectionService voice) => _voice = voice;

		[Command]
		[Summary("play a sound effect")]
		[Priority(0)]
		public async Task SpeakCommand([Summary("the filename")] string filename)
		{
			var files = Directory.GetFiles(BaseAudioPath, $"*.{AudioExt}", SearchOption.AllDirectories);

			var path = files.FirstOrDefault(f =>
				Path.GetRelativePath(BaseAudioPath, f) == $"{filename}.{AudioExt}");

			if (string.IsNullOrWhiteSpace(path))
			{
				// file not found
				await ReplyAsync("This isn't the file you are looking for");
				return;
			}

			using var channelAcquirer = await VoiceConnectionService.ChannelAcquirer.Create(_voice, Context);
			var voiceInfo = channelAcquirer.VoiceInfo;
			if (voiceInfo is null) //failed to connect
				return;

			var pcm = (await File.ReadAllBytesAsync(path)).NormalizeAudio().ToArray();
			await voiceInfo.AudioStream.WriteAsync(pcm);
		}

		[Command("list")]
		[Summary("list available sound files")]
		[Priority(1)]
		public async Task ListSpeakCommand()
		{
			var files = new DirectoryInfo(BaseAudioPath).GetFileTree($"*.{AudioExt}")
				.Skip(1); // skip name of top directory
			await ReplyAsync($"Available files are:{string.Join('\n', files).AsCode(true)}");
		}
	}
}