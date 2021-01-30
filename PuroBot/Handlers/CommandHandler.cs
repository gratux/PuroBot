using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PuroBot.Services;

namespace PuroBot.Handlers
{
	public class Initialize
	{
		private readonly CommandService _commands;
		private readonly DiscordSocketClient _client;
		public Initialize(CommandService commands = null, DiscordSocketClient client = null)
		{
			_commands = commands ?? new CommandService();
			_client = client ?? new DiscordSocketClient();
		}

		public IServiceProvider BuildServiceProvider() => new ServiceCollection()
			.AddSingleton(_client)
			.AddSingleton(_commands)
			.AddSingleton<CommandHandler>()
			.AddSingleton<LoggingService>()
			.AddSingleton<VoiceService>()
			.BuildServiceProvider();
	}
	public class CommandHandler
	{
		private readonly IServiceProvider _services;
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		
		private readonly Dictionary<ulong, char> _prefixes; // TODO: json interaction

		public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services, Dictionary<ulong, char> prefixes = null)
		{
			_client = client;
			_commands = commands;
			_services = services;
			_prefixes = prefixes ?? new Dictionary<ulong, char>();
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

			// Create a number to track where the prefix ends and the command begins
			var argPos = 0;

			var guildId = ((SocketGuildChannel) message.Channel).Guild.Id;
			if (!_prefixes.TryGetValue(guildId, out var prefix))
				prefix = '~';

			// Determine if the message is a command based on the prefix and make sure no bots trigger commands
			if (!(message.HasCharPrefix(prefix, ref argPos) ||
			      message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
			    message.Author.IsBot)
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