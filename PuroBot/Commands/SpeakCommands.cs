using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.VoiceNext;

namespace PuroBot.Commands
{
	[Group("speak")]
	[Description("play a sound file")]
	public class SpeakCommands : SoundCommands
	{
		private const string BaseAudioPath = "Resources/SpeakAudio/";
		private const string AudioExt = "pcm";
		
		[GroupCommand]
		public async Task SpeakCommand(CommandContext ctx, [Description("the filename")] string filename)
		{
			(Program.SoundTimeoutManager).AddOrUpdate(ctx.Guild);

			var vnext = ctx.Client.GetVoiceNext();
			var connection = vnext.GetConnection(ctx.Guild);
			if (connection == null)
			{
				await JoinCommand(ctx);
				connection = vnext.GetConnection(ctx.Guild);
			}

			var transmit = connection.GetTransmitSink();
			
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
				await ctx.RespondAsync("This isn't the file you are looking for");
				return;
			}

			var pcm = File.Open(path, FileMode.Open);
			await pcm.CopyToAsync(transmit);
			await pcm.DisposeAsync();
		}

		[Command("list")]
		[Description("list available sound files")]
		public async Task ListSpeakCommand(CommandContext ctx)
		{
			var files = new DirectoryInfo(BaseAudioPath).GetFileTree($"*.{AudioExt}")
				.Skip(1); // skip name of resource directory
			await ctx.RespondAsync($"Available files are:```\n{string.Join('\n', files)}\n```");
		}
	}
}