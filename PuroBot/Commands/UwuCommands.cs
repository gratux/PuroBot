using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace PuroBot.Commands
{
	[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	internal class UwuCommands : BaseCommandModule
	{
		private static readonly string[] Kaomoji =
		{
			"(*^ω^)",
			"(◕‿◕✿)",
			"(◕ᴥ◕)",
			"ʕ•ᴥ•ʔ",
			"ʕ￫ᴥ￩ʔ",
			"(*^.^*)",
			"owo",
			"OwO",
			"(｡♥‿♥｡)",
			"uwu",
			"UwU",
			"(*￣з￣)",
			">w<",
			"^w^",
			"(つ✧ω✧)つ",
			"(/ =ω=)/"
		};

		private static readonly Random Rnd = new Random();

		private static string UwuTranslate(string msg)
		{
			msg = Regex.Replace(msg, @"(?:l|r)", "w");
			msg = Regex.Replace(msg, @"(?:L|R)", "W");
			msg = Regex.Replace(msg, @"n([aeiou])", "ny$1");
			msg = Regex.Replace(msg, @"N([aeiouAEIOU])", "Ny$1");
			msg = Regex.Replace(msg, @"ove", "uv");
			msg = Regex.Replace(msg, @"nd(?= |$)", "ndo");
			msg = Regex.Replace(msg, @"!+", $"{Kaomoji[Rnd.Next(Kaomoji.Length)]}");
			return msg;
		}

		// str = str.replace(/(?:l|r)/g, 'w');
		// str = str.replace(/(?:L|R)/g, 'W');
		// str = str.replace(/n([aeiou])/g, 'ny$1');
		// str = str.replace(/N([aeiou])|N([AEIOU])/g, 'Ny$1');
		// str = str.replace(/ove/g, 'uv');
		// str = str.replace(/nd(?= |$)/g, 'ndo');
		// str = str.replace(
		// /!+/g,
		// ` ${kaomoji[Math.floor(Math.random() * kaomoji.length)]}`
		// );

		private static async Task<bool> CheckMsgEmpty(CommandContext ctx, string msg)
		{
			if (!string.IsNullOrWhiteSpace(msg)) return false;
			await ctx.RespondAsync("Silly Human, your message is empty. I can't translate nothing...");
			return true;
		}

		#region uwuify

		[Command("uwufy")]
		[Aliases("uwu")]
		[Description("translate a message to uwu-speak")]
		private async Task UwuCommand(CommandContext ctx,
			[RemainingText] [Description("the message i should translate")]
			string message)
		{
			if (await CheckMsgEmpty(ctx, message))
				return;

			var translated = UwuTranslate(message);
			await ctx.RespondAsync($"I translated your message, human:\n> {translated}");
		}

		#endregion

		#region uwuifythis

		[Command("uwufythis")]
		[Aliases("uwuthis")]
		[Description("translates the mentioned message to uwu-speak")]
		private async Task UwuThisCommand(CommandContext ctx)
		{
			// this returns null, if the message was written before the bot was online. why?!
			var mentioned = ctx.Message.Reference?.Message.Content;
			if (await CheckMsgEmpty(ctx, mentioned))
				return;

			var translated = UwuTranslate(mentioned);
			await ctx.RespondAsync($"I translated your message:\n> {translated}");
		}

		#endregion
	}
}