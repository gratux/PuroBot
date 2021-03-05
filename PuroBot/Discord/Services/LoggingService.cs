using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using PuroBot.Logger;

namespace PuroBot.Discord.Services
{
	public static class LoggingService
	{
		private static readonly ILogger Logger = new DefaultLogger();

		public static void Init(BaseDiscordClient client, CommandService command)
		{
			client.Log += Log;
			command.Log += Log;
		}

		private static string BuildMessage(LogMessage arg)
		{
			var msgBuilder = new StringBuilder();

			if (!string.IsNullOrWhiteSpace(arg.Message)) msgBuilder.Append(arg.Message);

			if (arg.Exception is null) return msgBuilder.ToString();

			if (msgBuilder.Length != 0)
			{
				msgBuilder.AppendFormat(": {0}", arg.Exception);
				return msgBuilder.ToString();
			}

			msgBuilder.Append(arg.Exception);
			return msgBuilder.ToString();
		}

		private static Task Log(LogMessage arg)
		{
			var msg = BuildMessage(arg);
			switch (arg.Severity)
			{
				case LogSeverity.Critical:
					Logger.LogError(msg, arg.Source);
					break;
				case LogSeverity.Error:
					Logger.LogError(msg, arg.Source);
					break;
				case LogSeverity.Warning:
					Logger.LogWarning(msg, arg.Source);
					break;
				case LogSeverity.Info:
					Logger.Log(msg, arg.Source);
					break;
				case LogSeverity.Verbose:
					Logger.LogDebug(msg, arg.Source);
					break;
				case LogSeverity.Debug:
					Logger.LogDebug(msg, arg.Source);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(arg));
			}

			return Task.CompletedTask;
		}
	}
}