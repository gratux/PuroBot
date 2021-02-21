using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Extensions;
using PuroBot.Services;

namespace PuroBot.CommandModules
{
	public class HelpModule : ModuleBase<SocketCommandContext>
	{
		private readonly CommandService _command;

		public HelpModule(CommandService command) => _command = command;

		[Command("help")]
		[Summary("show commands and their usage")]
		public async Task HelpCommand()
		{
			var embed = new EmbedBuilder();

			var guildId = Context.Guild?.Id;
			var prefix = guildId is null ? '~' : ConfigService.Servers.FirstOrDefault(s => s.Id == guildId).Prefix;

			embed.AddField("Generic Info",
				"Bot Prefix".AsHeader() + $": {prefix.ToString().AsCode()} or bot mention");

			var allCommands = _command.Modules.SelectMany(m => m.Commands).ToArray();
			Array.Sort(allCommands,
				(x, y) => StringComparer.InvariantCulture.Compare(x.Aliases[0], y.Aliases[0]));

			var messageContext = Context.Channel switch
			{
				IGuildChannel _ => ContextType.Guild,
				IGroupChannel _ => ContextType.Group,
				IDMChannel _ => ContextType.DM,
				_ => throw new ArgumentOutOfRangeException()
			};

			foreach (var command in allCommands)
			{
				var displayCommand = true;

				foreach (var precondition in command.Preconditions.Concat(command.Module.Preconditions))
				{
					if (precondition is not RequireContextAttribute attr) continue;

					// current context not in required contexts -> don't display
					if ((attr.Contexts & messageContext) == 0) displayCommand = false;
				}

				if (displayCommand)
					embed.AddField($"{prefix}{command.Aliases[0]}".AsCode(), await CommandHelp(command));
			}

			await ReplyAsync(embed: embed.Build());
		}

		private static Task<string> CommandHelp(CommandInfo command)
		{
			var name = command.Aliases[0];
			var summary = command.Summary ?? "No summary";
			var aliases = command.Aliases.Skip(1).ToArray();
			var parameterInfos = command.Parameters;
			var preconditions = command.Preconditions.Concat(command.Module.Preconditions)
				.Select(DiscordExtensions.DecodePrecondition).Distinct().ToArray();

			var sb = new StringBuilder();

			var parameterDesc = new List<string>();
			foreach (var p in parameterInfos)
			{
				if (p.IsOptional)
					sb.Append("[Optional] ");
				sb.AppendFormat("{0} <{1}>", p.Name.AsCode(), p.Type.ToString().AsCode());
				if (p.Summary != null)
					sb.AppendFormat(": {0}", p.Summary.AsItalic());
				parameterDesc.Add(sb.ToString());
				sb.Clear();
			}

			if (summary.Length != 0)
				sb.AppendLine(summary.AsItalic());
			sb.AppendFormat("{0}: {1}\n", "Command".AsHeader(), name.AsCode());
			sb.Append(aliases.IncludeIfAny("Aliases".AsHeader(), i => i.AsCode(), ", "));
			sb.Append(parameterDesc.IncludeIfAny("Parameters".AsHeader(), i => i.AsListItem(), "\n", true));
			sb.Append(preconditions.IncludeIfAny("Preconditions".AsHeader(), i => i.AsListItem(), "\n", true));

			return Task.FromResult(sb.ToString());
		}
	}
}