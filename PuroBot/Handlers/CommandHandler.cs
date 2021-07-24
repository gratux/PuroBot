using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PuroBot.Common;
using PuroBot.Discord.Services;

namespace PuroBot.Handlers
{
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
			if (arg is not SocketUserMessage message) return;
			// ignore bots
			if (message.Author.IsBot) return;

			// Create a number to track where the prefix ends and the command begins
			var argPos = 0;
			// get the command prefix to look for, depending on calling context
			var prefix = message.Channel is SocketGuildChannel guildChannel
				? GetOrAddServerPrefix(guildChannel.Guild.Id)
				: Settings.DefaultPrefix;

			// Determine if the message is a command based on the prefix
			if (!(message.HasStringPrefix(prefix, ref argPos) ||
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

		private string GetOrAddServerPrefix(ulong guildId) =>
			_services.GetService<DatabaseService>()?.GetServerCommandPrefix(guildId) ?? Settings.DefaultPrefix;
	}
}