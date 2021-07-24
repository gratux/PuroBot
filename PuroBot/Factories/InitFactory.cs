using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PuroBot.Common;
using PuroBot.Discord.Services;
using PuroBot.Handlers;

namespace PuroBot.Factories
{
	public class InitFactory
	{
		private DiscordSocketClient _client = new();
		private CommandService _commands = new();

		private InitFactory()
		{
			/* factory */
		}

		public static InitFactory Initialize(out CommandService commands, out DiscordSocketClient client)
		{
			client = new DiscordSocketClient(new DiscordSocketConfig
			{
				ExclusiveBulkDelete = true
			});
			commands = new CommandService(new CommandServiceConfig
			{
				CaseSensitiveCommands = false,
				DefaultRunMode = RunMode.Async,
				IgnoreExtraArgs = true,
				LogLevel = LogSeverity.Info
			});
			commands.CommandExecuted += EventHandlers.CmdExecutedHandler;

			client.Log += LogAsync;
			commands.Log += LogAsync;

			return new InitFactory {_client = client, _commands = commands};
		}

		public IServiceProvider BuildServiceProvider() =>
			new ServiceCollection()
				.AddSingleton(_client)
				.AddSingleton(_commands)
				.AddSingleton<VoiceConnectionService>()
				.AddSingleton<SpeechService>()
				.AddSingleton<DatabaseService>()
				.BuildServiceProvider();

		private static Task LogAsync(LogMessage arg)
		{
			switch (arg.Severity)
			{
				case LogSeverity.Critical:
					Logging.Fatal(arg.Message, arg.Exception, arg.Source);
					break;
				case LogSeverity.Error:
					Logging.Error(arg.Message, arg.Exception, arg.Source);
					break;
				case LogSeverity.Warning:
					Logging.Warn(arg.Message, arg.Exception, arg.Source);
					break;
				case LogSeverity.Info:
					Logging.Info(arg.Message, arg.Exception, arg.Source);
					break;
				case LogSeverity.Verbose: goto case LogSeverity.Debug;
				case LogSeverity.Debug:
					Logging.Debug(arg.Message, arg.Exception, arg.Source);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(arg));
			}

			return Task.CompletedTask;
		}
	}
}