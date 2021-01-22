using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;
using PuroBot.Commands;

namespace PuroBot
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			// if (args?.Length == 0)
			// {
			// 	Console.WriteLine("Please provide the bot token as an argument");
			// 	return;
			// }
			//
			// var token = args?[0];
			var token = File.ReadAllText("botkey.txt").Trim();
			MainAsync(token).GetAwaiter().GetResult();
		}

		private static async Task MainAsync(string token)
		{
			var discord = new DiscordClient(new DiscordConfiguration
			{
				Token = token,
				TokenType = TokenType.Bot,
				Intents = DiscordIntents.AllUnprivileged
				          | DiscordIntents.GuildMembers
			});

			discord.UseVoiceNext();

			var commands = discord.UseCommandsNext(new CommandsNextConfiguration
			{
				StringPrefixes = new[] {"~"}
			});

			commands.RegisterCommands<BasicCommands>();
			commands.RegisterCommands<UwuCommands>();
			commands.RegisterCommands<ImageCommands>();
			commands.RegisterCommands<JaySayCommands>();
			commands.RegisterCommands<SoundCommands>();

			// discord.MessageCreated += (sender, args) =>
			// {
			// 	_ = Task.Run(() => RoleEvents.RoleManager(args));
			// 	return Task.CompletedTask;
			// };

			await discord.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}