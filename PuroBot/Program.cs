using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PuroBot.Common;
using PuroBot.Factories;
using PuroBot.Handlers;

namespace PuroBot
{
	internal static class Program
	{
		private static CommandHandler? _commandHandler;

		private static void Main()
		{
			var token = Secrets.BotToken;
			MainAsync(token).GetAwaiter().GetResult();
		}

		private static async Task MainAsync(string token)
		{
			IServiceProvider services = InitFactory.Initialize(out CommandService commands,
					out DiscordSocketClient client)
				.BuildServiceProvider();

			_commandHandler = new CommandHandler(client, commands, services);

			await _commandHandler.InstallCommandsAsync();

			await client.LoginAsync(TokenType.Bot, token);
			await client.StartAsync();

			await Task.Delay(-1);
		}
	}
}