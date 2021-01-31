using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Extensions;

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

			builder.AddField("Generic Info", "Bot Prefix".AsHeader() + $": {prefix.AsCode()} or bot mention");

			var allCommands = _command.Modules.SelectMany(m => m.Commands).ToArray();
			Array.Sort(allCommands,
				(a, b) => StringComparer.InvariantCulture.Compare(a.Aliases.First(), b.Aliases.First()));

			foreach (var command in allCommands)
			{
				builder.AddField($"{prefix}{command.Aliases.First()}".AsCode(), await CommandHelp(command));
			}

			await ReplyAsync(embed: builder.Build());
		}

		private static Task<string> CommandHelp(CommandInfo command)
		{
			var name = command.Aliases.First();
			var summary = command.Summary ?? "No summary";
			var aliases = command.Aliases.Skip(1).ToArray();
			var parameterInfos = command.Parameters;
			var parameterDesc =
				parameterInfos.Select(p =>
						$"{(p.IsOptional ? "[Optional] " : null)}{p.Name.AsCode()} {("<" + p.Type + ">").AsCode()}: {(p.Summary ?? "No description").AsItalic()}")
					.ToArray();
			var preconditions = command.Preconditions.Select(p => p.GetType().Name).ToArray();

			var commandHelp = $"{summary.AsItalic()}\n"
			                  + "Command".AsHeader() + $": {name.AsCode()}\n"
			                  + aliases.IncludeIfAny("Aliases".AsHeader(), i => i.AsCode(), ", ")
			                  + parameterDesc.IncludeIfAny("Parameters".AsHeader(), i => i.AsListItem(), "\n", true)
			                  + preconditions.IncludeIfAny("Preconditions".AsHeader(), i => i.AsListItem(), "\n", true);

			return Task.FromResult(commandHelp);
		}
	}
}