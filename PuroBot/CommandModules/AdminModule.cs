using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.StaticServices;

namespace PuroBot.CommandModules
{
	[Summary("commands for server administration")]
	[RequireUserPermission(GuildPermission.Administrator)]
	public class AdminModule : ModuleBase<SocketCommandContext>
	{
		[Command("clear")]
		[Summary("delete multiple messages")]
		public async Task ClearCommand([Summary("the number of messages to clear")]
			int count)
		{
			await Context.Message.DeleteAsync();

			var messages = (await Context.Channel.GetMessagesAsync(count).FlattenAsync()).ToArray();
			(Context.Channel as ITextChannel)?.DeleteMessagesAsync(messages);

			var responseTask = ReplyAsync($"Deleted {messages.Length} message{(messages.Length != 1 ? "s" : "")}");
			await responseTask;
			await Task.Delay(5000);
			await responseTask.Result.DeleteAsync();
		}

		[Command("setpfx")]
		[Summary("set a new command prefix")]
		public async Task SetPrefixCommand([Summary("the new prefix")] char prefix)
		{
			var server = ConfigService.Servers.FirstOrDefault(s => s.Id == Context.Guild.Id);
			ConfigService.Servers.Remove(server);
			server.Prefix = prefix;
			ConfigService.Servers.Add(server);
			await ReplyAsync($"Set new prefix \"{prefix}\"");
		}
	}
}