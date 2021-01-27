using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;
using PuroBot.Commands;
using PuroBot.Events;

namespace PuroBot
{
	internal static class Program
	{
		public static SoundTimeoutManager SoundTimeoutManager;

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

			SoundTimeoutManager = new SoundTimeoutManager(discord);

			discord.UseVoiceNext();

			var commands = discord.UseCommandsNext(new CommandsNextConfiguration
			{
				StringPrefixes = new[] {"~"}
			});

			commands.RegisterCommands<BasicCommands>();
			commands.RegisterCommands<UwuCommands>();
			commands.RegisterCommands<ImageCommands>();
			commands.RegisterCommands<ReactionCommands>();
			commands.RegisterCommands<SoundCommands>();
			commands.RegisterCommands<SpeakCommands>();

			commands.CommandErrored += async (ext, args) => await EventHandlers.CmdErroredHandler(ext, args);

			await discord.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}