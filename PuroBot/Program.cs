using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PuroBot.Handlers;
using PuroBot.StaticServices;

namespace PuroBot
{
	internal static class Program
	{
		private static CommandHandler _commandHandler;

		private static void Main()
		{
			var token = ConfigService.Global.Token;
			MainAsync(token).GetAwaiter().GetResult();
		}

		private static async Task MainAsync(string token)
		{
			var client = new DiscordSocketClient(new DiscordSocketConfig
			{
				ExclusiveBulkDelete = true
			});
			client.Ready += () => EventHandlers.ClientReadyHandler(client);
			var commands = new CommandService(new CommandServiceConfig
			{
				CaseSensitiveCommands = false,
				DefaultRunMode = RunMode.Async,
				IgnoreExtraArgs = true,
				LogLevel = LogSeverity.Warning
			});
			commands.CommandExecuted += EventHandlers.CmdExecutedHandler;

			LoggingService.Init(client, commands);

			_commandHandler = new CommandHandler(client, commands,
				new Initialize(commands, client).BuildServiceProvider());

			await _commandHandler.InstallCommandsAsync();

			await client.LoginAsync(TokenType.Bot, token);
			await client.StartAsync();

			await Task.Delay(-1);
		}
	}
}