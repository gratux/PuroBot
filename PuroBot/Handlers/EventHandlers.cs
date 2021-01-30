using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Services;

namespace PuroBot.Handlers
{
	public static class EventHandlers
	{
		public static async Task CmdExecutedHandler(Optional<CommandInfo> info, ICommandContext context, IResult result)
		{
			if (result.IsSuccess)
			{
				if (info.IsSpecified) // if possible, log executed command
					await LoggingService.LogAsync(new LogMessage(LogSeverity.Info, "command",
						$"{info.Value.Name} executed"));
				return;
			}

			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (result.Error)
			{
				case CommandError.UnknownCommand:
					await context.Message.ReplyAsync($"I don't know this command.");
					break;
				// case CommandError.ParseFailed:
				// 	break;
				case CommandError.BadArgCount:
					await context.Message.ReplyAsync("Some command parameters are missing.");
					break;
				// case CommandError.ObjectNotFound:
				// 	break;
				// case CommandError.MultipleMatches:
				// 	break;
				// case CommandError.UnmetPrecondition:
				// 	break;
				// case CommandError.Exception:
				// 	break;
				// case CommandError.Unsuccessful:
				// 	break;
				default:
					if (info.IsSpecified)
						await LoggingService.LogAsync(new LogMessage(LogSeverity.Error, "command",
							$"{info.Value.Name} errored",
							new CommandException(info.Value, context, new Exception(result.ErrorReason))));
					break;
			}
		}
	}
}