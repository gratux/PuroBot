using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PuroBot.Config;
using PuroBot.Services;
using PuroBot.StaticServices;

namespace PuroBot.Handlers
{
	public class Initialize
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;

		public Initialize(CommandService commands = null, DiscordSocketClient client = null)
		{
			_commands = commands ?? new CommandService();
			_client = client ?? new DiscordSocketClient();
		}

		public IServiceProvider BuildServiceProvider() =>
			new ServiceCollection()
				.AddSingleton(_client)
				.AddSingleton(_commands)
				.AddSingleton<CommandHandler>()
				.AddSingleton<VoiceService>()
				.AddTransient<SpeechService>()
				.BuildServiceProvider();
	}

	public class CommandHandler
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;

		public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
		{
			_client = client;
			_commands = commands;
			_services = services;
		}

		public async Task InstallCommandsAsync()
		{
			_client.MessageReceived += HandleCommandAsync;
			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
		}

		private async Task HandleCommandAsync(SocketMessage arg)
		{
			// Don't process the command if it was a system message
			if (!(arg is SocketUserMessage message)) return;
			// ignore bots
			if (message.Author.IsBot) return;

			// Create a number to track where the prefix ends and the command begins
			var argPos = 0;

			var guildId = ((SocketGuildChannel) message.Channel).Guild.Id;

			char prefix;
			try
			{
				prefix = ConfigService.Servers.First(s => s.Id == guildId).Prefix;
			}
			catch (InvalidOperationException)
			{
				// server not in config -> add with default value
				prefix = '~';
				ConfigService.Servers.Add(new Server
				{
					Id = guildId,
					Prefix = prefix
				});
			}

			// Determine if the message is a command based on the prefix
			if (!(message.HasCharPrefix(prefix, ref argPos) ||
			      message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
				return;

			// Create a WebSocket-based command context based on the message
			var context = new SocketCommandContext(_client, message);

			// Execute the command with the command context we just
			// created, along with the service provider for precondition checks.
			await _commands.ExecuteAsync(
				context,
				argPos,
				_services);
		}
	}
}