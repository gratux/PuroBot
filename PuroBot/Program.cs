using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace PuroBot
{
    class Program
    {
        private const string prefix = "~"; 
        static void Main(string[] args)
        {
            if (args?.Length == 0)
            {
                Console.WriteLine("Please provide the bot token as an argument");
                return;
            }

            var token = args?[0];
            MainAsync(token).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string token)
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += async (sender, args) =>
            {
                if (args.Message.Content.ToLower().Trim().StartsWith(prefix))
                    await ProcessCommand(args);
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task ProcessCommand(MessageCreateEventArgs args)
        {
            var command = args.Message.Content.Substring(prefix.Length).TrimStart();
            switch (command)
            {
                case "ping":
                    await args.Message.RespondAsync("Hello Human!");
                    break;
                case "selfie": case "s":
                    await args.Message.RespondAsync("My camera is broken... Sorry");
                    break;
                default:
                    await args.Message.RespondAsync("I don't understand your request, human...");
                    break;
            }
        }
    }
}