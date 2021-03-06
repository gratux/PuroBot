using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Extensions;

namespace PuroBot.Discord.CommandModules
{
	public class UwuModule : ModuleBase<SocketCommandContext>
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

		private static readonly Random Rnd = new();

		private static string UwuTranslate(string msg)
		{
			msg = Regex.Replace(msg, @"(?:l|r)", "w");
			msg = Regex.Replace(msg, @"(?:L|R)", "W");
			msg = Regex.Replace(msg, @"n([aeiou])", "ny$1");
			msg = Regex.Replace(msg, @"N([aeiouAEIOU])", "Ny$1");
			msg = Regex.Replace(msg, @"ove", "uv");
			msg = Regex.Replace(msg, @"nd(?= |$)", "ndo");
			msg = Regex.Replace(msg, @"!+", _=>Kaomoji[Rnd.Next(Kaomoji.Length)].AsCode());
			return msg;
		}

		private static async Task<bool> CheckMsgEmpty(ICommandContext ctx, string msg)
		{
			if (!string.IsNullOrWhiteSpace(msg)) return false;
			await ctx.Message.ReplyAsync("Silly Human, your message is empty. I can't translate nothing...");
			return true;
		}

		[Command("uwufy")]
		[Alias("uwu")]
		[Summary("translate a message to uwu-speak")]
		public async Task UwuCommand(
			[Remainder] [Summary("the message i should translate")]
			string message)
		{
			if (await CheckMsgEmpty(Context, message))
				return;

			var translated = UwuTranslate(message);
			await ReplyAsync($"I translated your message, human:\n>>> {translated}");
		}

		[Command("uwufythis")]
		[Alias("uwuthis")]
		[Summary("translates the mentioned message to uwu-speak")]
		public async Task UwuThisCommand()
		{
			// this returns null, if the message was written before the bot was online. why?!
			var mentioned = Context.Message.ReferencedMessage.Content;
			if (await CheckMsgEmpty(Context, mentioned))
				return;

			var translated = UwuTranslate(mentioned);
			await ReplyAsync($"I translated your message:\n>>> {translated}");
		}
	}
}