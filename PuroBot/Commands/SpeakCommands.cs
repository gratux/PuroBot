using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.VoiceNext;

namespace PuroBot.Commands
{
	[Group("speak")]
	[Description("play a sound file")]
	[RequireRoles(RoleCheckMode.All, "PuroSpeak")]
	public class SpeakCommands : SoundCommands
	{
		private const string BaseAudioPath = "Resources/SpeakAudio/";
		private const string AudioExt = "pcm";
		private readonly Semaphore _sp = new Semaphore(1, 1);

		[GroupCommand]
		public async Task SpeakCommand(CommandContext ctx, [Description("the filename")] string filename)
		{
			// var user = ctx.Member;
			// if (user.Roles.All(r => r.Name != "Papst"))
			// 	throw new ChecksFailedException(ctx.Command, ctx,
			// 		new[] {new RequireRolesAttribute(RoleCheckMode.All, "Papst")});

			Program.SoundTimeoutManager.AddOrUpdate(ctx.Guild);

			await JoinCommand(ctx);

			var vnext = ctx.Client.GetVoiceNext();
			var connection = vnext.GetConnection(ctx.Guild);

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

			_sp.WaitOne();
			try
			{
				var pcm = File.Open(path, FileMode.Open);
				await pcm.CopyToAsync(transmit);
				await pcm.DisposeAsync();
			}
			finally
			{
				_sp.Release();
			}
		}

		[Command("list")]
		[Description("list available sound files")]
		// [RequireRoles(RoleCheckMode.All, "Papst")]
		public async Task ListSpeakCommand(CommandContext ctx)
		{
			var files = new DirectoryInfo(BaseAudioPath).GetFileTree($"*.{AudioExt}")
				.Skip(1); // skip name of resource directory
			await ctx.RespondAsync($"Available files are:```\n{string.Join('\n', files)}\n```");
		}
	}
}