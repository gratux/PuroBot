using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace PuroBot.Events
{
	internal static class BasicEvents
	{
		public static async Task LostCrusader(MessageCreateEventArgs e)
		{
			//ignore bots, they might have a reason to be here
			if (e.Author.IsBot)
				return;

			var allRoles = e.Guild.Roles.Values.ToArray();
			//var allMembers = e.Guild.Members.Values.ToArray(); //doesn't get all members; why?!
			var allMembers = e.Guild.GetAllMembersAsync().Result.ToArray();

			//why are DiscordMember and DiscordUser two different things ?!
			var authorMember = allMembers.First(member => member.Id == e.Author.Id);

			//hard-coded value for a specific server
			//(then again, this whole function serves no purpose for other servers)
			//this is not clean at all
			//too bad...
			var crusaderRole = allRoles.FirstOrDefault(role => role.Name == "Crusader");
			var purifierRole = allRoles.FirstOrDefault(role => role.Name == "The Purifiers");
			var hereticRole = allRoles.FirstOrDefault(role => role.Name == "Heretic");
			var changedRole = allRoles.FirstOrDefault(role => role.Name == "The Changed");

			//if author is not a crusader, abort
			if (!authorMember.Roles.Any(role => role.Equals(crusaderRole) || role.Equals(purifierRole)))
				return;

			var allOverwrites = e.Channel.PermissionOverwrites.ToArray();
			var roleOverwrites = allOverwrites.Where(ow => ow.Type == OverwriteType.Role).ToArray();

			var isHereticChannel = false;

			//determine if current channel belongs to the heretics
			//criteria are: heretics have access, but crusaders do not
			foreach (var overwrite in roleOverwrites)
			{
				var owRole = await overwrite.GetRoleAsync(); //this takes very long. why?! is there no better way?!
				var owPermission = overwrite.CheckPermission(Permissions.AccessChannels);

				if (owRole.Id == crusaderRole?.Id && owPermission == PermissionLevel.Allowed ||
				    owRole.Id == purifierRole?.Id && owPermission == PermissionLevel.Allowed
				) //crusaders are granted access, not heretic channel
				{
					isHereticChannel = false;
					break;
				}

				if (!(owRole.Id == hereticRole?.Id && owPermission == PermissionLevel.Allowed ||
				      owRole.Id == changedRole?.Id && owPermission == PermissionLevel.Allowed)
				) //heretics have access; let's hope the crusaders don't
					isHereticChannel = true;
			}

			if (isHereticChannel)
			{
				await e.Message.DeleteAsync(":MagicFox:");
				var e621 = new E621Client();
				var urls = e621.GetPostUrls("reaction_image rating:s order:random", 10).Result;

				if (urls.Count > 0)
				{
					var responseTask =
						e.Message.RespondAsync(
							"Bad human, this channel is not meant for you. Have some heresy as punishment!");
					//var heresyTask = authorMember.SendMessageAsync(string.Join('\n', urls));
					var heresyTask = Helpers.SendMany(urls, msg => authorMember.SendMessageAsync(msg));
					Task.WaitAll(responseTask, heresyTask);
					await Task.Delay(5000);
					await responseTask.Result.DeleteAsync();
				}
			}
		}
	}
}