using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace PuroBot.Modules
{
	public class HelpModule : ModuleBase<SocketCommandContext>
	{
		private CommandService _command;

		public HelpModule(CommandService command)
		{
			_command = command;
		}
		
		[Command("help")]
		public async Task HelpCommand(string group = null)
		{
			var commands = _command.Commands;
			var helpInfo = commands.Select(c => $"{c.Module.Group}/{c.Name} - {c.Summary}");
			await ReplyAsync(string.Join('\n', helpInfo));
		}
	}
}