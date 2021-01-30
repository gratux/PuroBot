using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;
using Discord.Commands;
using PuroBot.Services;

namespace PuroBot.Modules
{
	[Group("speak")]
	[Summary("play a sound file")]
	public class SpeakModule : ModuleBase<SocketCommandContext>
	{
		private readonly VoiceService _voice;
		
		private const string BaseAudioPath = "Resources/SpeakAudio/";
		private const string AudioExt = "pcm";
		private static readonly Semaphore Sp = new Semaphore(1, 1);

		public SpeakModule(VoiceService voice)
		{
			_voice = voice;
		}

		[Command]
		[Priority(0)]
		public async Task SpeakCommand([Summary("the filename")] string filename)
		{
			var audioInfo = await _voice.JoinChannel(Context);
			var audioStream = audioInfo.AudioStream;
			
			string path;
			try
			{
				var files = Directory.GetFiles(BaseAudioPath, $"*.{AudioExt}", SearchOption.AllDirectories);

				path = files.First(f =>
					Path.GetRelativePath(BaseAudioPath, f) == $"{filename}.{AudioExt}");
			}
			catch (InvalidOperationException)
			{
				// file not found
				await ReplyAsync("This isn't the file you are looking for");
				return;
			}

			Sp.WaitOne();
			try
			{
				await using var fileStream = File.Open(path, FileMode.Open);
				await fileStream.CopyToAsync(audioStream);
				await audioStream.FlushAsync();
			}
			finally
			{
				Sp.Release();
			}
		}

		[Command("list")]
		[Summary("list available sound files")]
		[Priority(1)]
		public async Task ListSpeakCommand()
		{
			var files = new DirectoryInfo(BaseAudioPath).GetFileTree($"*.{AudioExt}")
				.Skip(1); // skip name of resource directory
			await ReplyAsync($"Available files are:```\n{string.Join('\n', files)}\n```");
		}
	}
}