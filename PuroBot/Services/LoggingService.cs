using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;

namespace PuroBot.Services
{
	public static class LoggingService
	{
		public static void Init(BaseDiscordClient client, CommandService command)
		{
			client.Log += Log;
			command.Log += Log;
		}

		public static Task Log(LogSeverity severity, string source, string message, Exception exception = null) =>
			Log(new LogMessage(severity, source, message, exception));

		private static Task Log(LogMessage msg)
		{
			switch (msg.Exception)
			{
				case CommandException ex:
					Console.WriteLine(
						$"[{DateTime.Now} - Command/{msg.Severity}] {ex.Command.Aliases[0]} failed to execute in {ex.Context.Channel}.");
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