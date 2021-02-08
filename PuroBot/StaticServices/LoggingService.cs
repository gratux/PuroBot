using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;

namespace PuroBot.StaticServices
{
	public static class LoggingService
	{
		public static void Init(BaseDiscordClient client, CommandService command)
		{
			client.Log += LogAsync;
			command.Log += LogAsync;
		}

		public static Task LogAsync(LogMessage msg)
		{
			switch (msg.Exception)
			{
				case CommandException ex:
					Console.WriteLine(
						$"[{DateTime.Now} - Command/{msg.Severity}] {ex.Command.Aliases.First()} failed to execute in {ex.Context.Channel}.");
					Console.WriteLine(ex);
					break;
				default:
					Console.WriteLine($"[{DateTime.Now} - General/{msg.Severity}] {msg}.");
					break;
			}

			return Task.CompletedTask;
		}
	}
}