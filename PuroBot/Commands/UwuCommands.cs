using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace PuroBot.Commands
{
	[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	internal class UwuCommands : BaseCommandModule
	{
		private static string UwuTranslate(string msg)
		{
			return msg.Replace('l', 'w').Replace('r', 'w');
		}

		private static async Task<bool> CheckMsgEmpty(CommandContext ctx, string msg)
		{
			if (!string.IsNullOrWhiteSpace(msg)) return false;
			await ctx.RespondAsync("Silly Human, your message is empty. I can't translate nothing...");
			return true;
		}

		#region uwuify

		[Command("uwuify")]
		[Aliases("uwu")]
		[Description("translate a message to uwu-speak")]
		private async Task UwuCommand(CommandContext ctx,
			[RemainingText] [Description("the message i should translate")]
			string message)
		{
			await UwuCommand(ctx, false, message);
		}

		[Command("uwuify")]
		[Description("translate a message to uwu-speak")]
		private async Task UwuCommand(CommandContext ctx, [Description("speak this message with TTS")]
			bool tts, [RemainingText] [Description("the message i should translate")]
			string message)
		{
			if (await CheckMsgEmpty(ctx, message))
				return;

			var translated = UwuTranslate(message);
			await ctx.RespondAsync("I translated your message, human:");
			await ctx.RespondAsync($"> {translated}", tts);
		}

		#endregion

		#region uwuifythis

		[Command("uwuifythis")]
		[Aliases("uwuthis")]
		[Description("translates the mentioned message to uwu-speak")]
		private async Task UwuThisCommand(CommandContext ctx)
		{
			await UwuThisCommand(ctx, false);
		}

		[Command("uwuifythis")]
		[Description("translates the mentioned message to uwu-speak")]
		private async Task UwuThisCommand(CommandContext ctx, [Description("speak this message with TTS")]
			bool tts)
		{
			// this returns null, if the message was written before the bot was online. why?!
			var mentioned = ctx.Message.Reference?.Message.Content;
			if (await CheckMsgEmpty(ctx, mentioned))
				return;

			var translated = UwuTranslate(mentioned);
			await ctx.RespondAsync("I translated your message:");
			await ctx.RespondAsync($"> {translated}", tts);
		}

		#endregion
	}
}