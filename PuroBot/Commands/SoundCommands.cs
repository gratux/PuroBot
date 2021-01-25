using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace PuroBot.Commands
{
	public class SoundCommands : BaseCommandModule
	{
		private const string BaseAudioPath = "Resources/SpeakAudio/";
		private const string AudioExt = "pcm";

		private static SoundTimeoutManager _timeoutManager;

		[Command("join")]
		[Description("join the current voice channel")]
		public async Task JoinCommand(CommandContext ctx, DiscordChannel channel = null)
		{
			(_timeoutManager ??= new SoundTimeoutManager(ctx.Client)).AddOrUpdate(ctx.Guild);

			channel ??= ctx.Member.VoiceState?.Channel;
			await channel.ConnectAsync();
		}

		[Command("leave")]
		[Description("leave the voice channel")]
		public async Task LeaveCommand(CommandContext ctx)
		{
			(_timeoutManager ??= new SoundTimeoutManager(ctx.Client)).Remove(ctx.Guild);

			var vnext = ctx.Client.GetVoiceNext();
			var connection = vnext?.GetConnection(ctx.Guild);

			connection?.Disconnect();
		}

		[Command("speak")]
		[Description("play a sound file")]
		public async Task SpeakCommand(CommandContext ctx, [Description("the filename")] string filename)
		{
			(_timeoutManager ??= new SoundTimeoutManager(ctx.Client)).AddOrUpdate(ctx.Guild);

			var vnext = ctx.Client.GetVoiceNext();
			var connection = vnext.GetConnection(ctx.Guild);
			if (connection == null)
			{
				await JoinCommand(ctx);
				connection = vnext.GetConnection(ctx.Guild);
			}

			var transmit = connection.GetTransmitSink();
// var files = Directory.GetFiles(BaseAudiopath, $"*.{AudioExt}")
			// 	.Select(path => $"> {Path.GetFileNameWithoutExtension(path)}");
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

		[Command("listspeak")]
		[Description("list available sound files")]
		public async Task ListSpeakCommand(CommandContext ctx)
		{
			var files = new DirectoryInfo(BaseAudioPath).GetFileTree($"*.{AudioExt}")
				.Skip(1);
			await ctx.RespondAsync($"Available files are:```\n{string.Join('\n', files)}\n```");
		}
	}
}