using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PuroBot.Common;
using PuroBot.Factories;
using PuroBot.Handlers;

namespace PuroBot
{
    internal static class Program
    {
        private static async Task Main()
        {
            string? token = Secrets.BotToken;
            IServiceProvider services = InitFactory.Initialize(out CommandService commands, out DiscordSocketClient client)
                .BuildServiceProvider();

            var commandHandler = new CommandHandler(client, commands, services);
            await commandHandler.InstallCommandsAsync();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
