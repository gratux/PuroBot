using System;
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

		private static Task Log(LogMessage arg)
		{
			switch (arg.Severity)
			{
				case LogSeverity.Critical:
					if (arg.Exception is not null)
					{
						Logger.LogError(arg.Exception, arg.Source);
						break;
					}

					Logger.LogError(arg.Message, arg.Source);
					break;
				case LogSeverity.Error:
					if (arg.Exception is not null)
					{
						Logger.LogError(arg.Exception, arg.Source);
						break;
					}

					Logger.LogError(arg.Message, arg.Source);
					break;
				case LogSeverity.Warning:
					Logger.LogWarning(arg.Message, arg.Source);
					break;
				case LogSeverity.Info:
					Logger.Log(arg.Message, arg.Source);
					break;
				case LogSeverity.Verbose:
					Logger.LogDebug(arg.Message, arg.Source);
					break;
				case LogSeverity.Debug:
					Logger.LogDebug(arg.Message, arg.Source);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(arg));
			}

			return Task.CompletedTask;
		}
	}
}