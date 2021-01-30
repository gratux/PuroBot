using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace PuroBot.Modules
{
	public class HelpModule : ModuleBase<SocketCommandContext>
	{
		private readonly CommandService _command;

		public HelpModule(CommandService command)
		{
			_command = command;
		}

		[Command("help")]
		[Summary("show commands and their usage")]
		public async Task HelpCommand()
		{
			var builder = new EmbedBuilder();

			const string prefix = "~";

			builder.AddField("Generic Info", $"__Bot Prefix__: `{prefix}` or bot mention");

			var allCommands = _command.Modules.SelectMany(m => m.Commands).ToArray();
			Array.Sort(allCommands,
				(a, b) => StringComparer.InvariantCulture.Compare(a.Aliases.First(), b.Aliases.First()));

			foreach (var command in allCommands)
			{
				builder.AddField($"`{prefix}{command.Aliases.First()}`", await CommandHelp(command));
			}

			await ReplyAsync(embed: builder.Build());
		}

		private static async Task<string> CommandHelp(CommandInfo command)
		{
			var name = command.Aliases.First();
			var summary = command.Summary ?? "No summary";
			var aliases = command.Aliases.Skip(1).ToArray();
			var parameterInfos = command.Parameters;
			var parameterDesc =
				parameterInfos.Select(p =>
						$"{(p.IsOptional ? "[*Optional*] " : "")}`{p.Name}` `<{p.Type}>`: *{p.Summary ?? "No description"}*")
					.ToArray();
			var preconditions = command.Preconditions.Select(p => p.GetType().Name).ToArray();

			var commandHelp = $"*{summary}*\n"
			                  + $"__Command__: `{name}`\n"
			                  + "__Aliases__: " + (aliases.Any()
				                  ? $"`{string.Join("`, `", aliases)}`"
				                  : "*none*") + "\n"
			                  + "__Parameters__:\n• " + (parameterDesc.Any()
				                  ? string.Join("\n• ", parameterDesc)
				                  : "*none*") + "\n"
			                  + "__Preconditions__:\n• " + (preconditions.Any()
				                  ? string.Join("\n• ", preconditions)
				                  : "*none*");

			return commandHelp;
		}
	}
}