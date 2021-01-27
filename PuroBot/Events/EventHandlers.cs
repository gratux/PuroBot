using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;

namespace PuroBot.Events
{
	public class EventHandlers
	{
		public static async Task CmdErroredHandler(CommandsNextExtension ext, CommandErrorEventArgs args)
		{
			switch (args.Exception)
			{
				case ChecksFailedException checksFailedException:
					var failedChecks = checksFailedException.FailedChecks;
					foreach (var failedCheck in failedChecks)
						switch (failedCheck)
						{
							case RequirePermissionsAttribute attribute:
								await args.Context.RespondAsync(
									$"You do not have the required permissions to execute this command:\n```{string.Join('\n', attribute.Permissions)}```");
								break;

							case RequireRolesAttribute attribute:
								await args.Context.RespondAsync(
									$"You do not have the required roles to execute this command:\n```{string.Join('\n', attribute.RoleNames)}```");
								break;

							default:
								await args.Context.RespondAsync(
									"You are not allowed to run this command. I don't quite know why.");
								break;
						}

					break;

				case CommandNotFoundException _:
					await args.Context.RespondAsync(
						$"I do not know this command. Use {args.Context.Prefix}help to list all commands");
					break;

				default:
					await args.Context.RespondAsync(
						"Either you did something wrong or I'm having a bad day. In either case, please tell my owner.");
					break;
			}
		}
	}
}