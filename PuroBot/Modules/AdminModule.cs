using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace PuroBot.Modules
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
			
			var responseTask = ReplyAsync($"Deleted {messages.Length} message{(messages.Length!=1?"s":"")}");
			await Task.Run(() =>
			{
				responseTask.Wait();
				Task.Delay(5000).Wait();
				responseTask.Result.DeleteAsync().Wait();
			});
		}
	}
}