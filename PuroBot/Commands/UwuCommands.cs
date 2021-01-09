using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace PuroBot.Commands
{
	public class UwuCommands : BaseCommandModule
	{
		[Command("uwuify"), Aliases("uwu"), Description("translate a message to uwu-speak")]
		async Task UwuCommand(CommandContext ctx, [RemainingText, Description("the message i should translate")]
			string message)
		{
			await UwuCommand(ctx, false, message);
		}
		
		[Command("uwuify"), Description("translate a message to uwu-speak")]
		async Task UwuCommand(CommandContext ctx, [Description("speak this message with TTS")]
			bool tts, [RemainingText, Description("the message i should translate")]
			string message)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				await ctx.RespondAsync("Silly Human, I need a message to translate.");
				return;
			}

			var translated = UwuTranslate(message);
			await ctx.RespondAsync("I translated your message, human:");
			await ctx.RespondAsync($"> {translated}", tts);
		}

		[Command("uwuifythis"), Aliases("uwuthis"), Description("translates the mentioned message to uwu-speak")]
		async Task UwuThisCommand(CommandContext ctx)
		{
			await UwuThisCommand(ctx, false);
		}
		
		[Command("uwuifythis"), Description("translates the mentioned message to uwu-speak")]
		async Task UwuThisCommand(CommandContext ctx, [Description("speak this message with TTS")]
			bool tts)
		{
			// this returns null, if the message was written before the bot was online. why?!
			var mentioned = ctx.Message.Reference?.Message.Content;
			if (string.IsNullOrWhiteSpace(mentioned))
			{
				await ctx.RespondAsync("Silly Human, I need a message to translate.");
				return;
			}

			var translated = UwuTranslate(mentioned);
			await ctx.RespondAsync($"I translated your message:");
			await ctx.RespondAsync($"> {translated}", tts);
		}

		string UwuTranslate(string msg)
		{
			return msg.Replace('l', 'w').Replace('r', 'w');
		}
	}
}