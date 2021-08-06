using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PuroBot.Discord.Services;
using PuroBot.Extensions;

namespace PuroBot.Discord.Modules
{
    [Summary("commands for server administration")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireContext(ContextType.Guild)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        public AdminModule(DatabaseService database)
        {
            _database = database;
        }

        [Command("clear")]
        [Summary("delete multiple messages")]
        public async Task ClearCommand([Summary("the number of messages to clear")] int count)
        {
            await Context.Message.DeleteAsync();

            var messages = (await Context.Channel.GetMessagesAsync(count).FlattenAsync()).ToArray();
            (Context.Channel as ITextChannel)?.DeleteMessagesAsync(messages);

            await DiscordExtensions.SendTemporary(
                $"Deleted {messages.Length} message{(messages.Length != 1 ? "s" : "")}",
                TimeSpan.FromSeconds(5),
                async msg => await ReplyAsync(msg));
        }

        [Command("setpfx")]
        [Summary("set a new command prefix")]
        public async Task SetPrefixCommand([Summary("the new prefix")] string prefix)
        {
            if (_database.SetServerCommandPrefix(Context.Guild.Id, prefix))
            {
                await ReplyAsync($"New command prefix {prefix.AsCode()} set");
                return;
            }

            await ReplyAsync("Failed to set new command prefix");
        }

        private readonly DatabaseService _database;
    }
}
