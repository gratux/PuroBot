using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using PuroBot.Commands;

namespace PuroBot
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args?.Length == 0)
			{
				Console.WriteLine("Please provide the bot token as an argument");
				return;
			}

			var token = args?[0];
			MainAsync(token).GetAwaiter().GetResult();
		}

		static async Task MainAsync(string token)
		{
			var discord = new DiscordClient(new DiscordConfiguration()
			{
				Token = token,
				TokenType = TokenType.Bot
			});

			var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
			{
				StringPrefixes = new[] {"~"}
			});

			commands.RegisterCommands(typeof(BasicCommands));

			await discord.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}