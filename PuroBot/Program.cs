using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PuroBot.Handlers;
using PuroBot.Services;

namespace PuroBot
{
	internal static class Program
	{
		private static CommandHandler _commandHandler;
		private static LoggingService _loggingService;

		private static void Main(string[] args)
		{
			// if (args?.Length == 0)
			// {
			// 	Console.WriteLine("Please provide the bot token as an argument");
			// 	return;
			// }
			//
			// var token = args?[0];
			var token = File.ReadAllText("botkey.txt").Trim();
			MainAsync(token).GetAwaiter().GetResult();
		}

		private static async Task MainAsync(string token)
		{
			var client = new DiscordSocketClient();
			var commands = new CommandService(new CommandServiceConfig
			{
				CaseSensitiveCommands = false,
				DefaultRunMode = RunMode.Async,
				IgnoreExtraArgs = true,
				LogLevel = LogSeverity.Warning
			});
			commands.CommandExecuted += EventHandlers.CmdExecutedHandler;
			
			_loggingService = new LoggingService(client, commands);
			
			_commandHandler = new CommandHandler(client, commands,
				new Initialize(commands, client).BuildServiceProvider());

			await _commandHandler.InstallCommandsAsync();

			await client.LoginAsync(TokenType.Bot, token);
			await client.StartAsync();

			await Task.Delay(-1);
		}
	}
}