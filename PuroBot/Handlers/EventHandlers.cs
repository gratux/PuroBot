using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Logger;

namespace PuroBot.Handlers
{
	public static class EventHandlers
	{
		private static readonly ILogger Logger = new DefaultLogger();

		public static async Task CmdExecutedHandler(Optional<CommandInfo> info, ICommandContext context, IResult result)
		{
			if (result.IsSuccess)
			{
				if (info.IsSpecified) // if possible, log executed command
					Logger.Log($"{info.Value.Name} executed successfully");
				return;
			}

			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (result.Error)
			{
				case CommandError.UnknownCommand:
					await context.Message.ReplyAsync("I don't know this command.");
					break;

				case CommandError.UnmetPrecondition:
				case CommandError.BadArgCount:
					await context.Message.ReplyAsync(result.ErrorReason);
					break;

				case CommandError.MultipleMatches:
					await context.Message.ReplyAsync("Command is ambiguous, multiple definitions match.");
					break;

				default:
					await context.Message.ReplyAsync(result.ErrorReason);
					if (info.IsSpecified)
						Logger.LogError(new CommandException(info.Value, context, new Exception(result.ErrorReason)));
					break;
			}
		}
	}
}