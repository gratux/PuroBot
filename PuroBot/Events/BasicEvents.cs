using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace PuroBot.Events
{
	public class BasicEvents
	{
		public static async Task DieCrusader(DiscordClient sender, MessageCreateEventArgs e)
		{
			if (e.Author.IsBot)
				return;

			var allRoles = e.Guild.Roles.Values.ToArray();
			var allMembers = e.Guild.Members.Values.ToArray();
			var allChannels = e.Guild.Channels.Values.ToArray();

			var crusaderRole = allRoles.FirstOrDefault(role => role.Name == "Crusader");
			var purifierRole = allRoles.FirstOrDefault(role => role.Name == "The Purifiers");
			var hereticRole = allRoles.FirstOrDefault(role => role.Name == "Heretic");
			var changedRole = allRoles.FirstOrDefault(role => role.Name == "The Changed");

			var crusaderMembers = allMembers.Where(member => member.Roles.Contains(crusaderRole)).ToArray();

			var hereticChannels = new List<DiscordChannel>();

			foreach (var channel in allChannels)
			{
				var allOverwrites = channel.PermissionOverwrites.ToArray();
				var roleOverwrites = allOverwrites.Where(ow => ow.Type == OverwriteType.Role).ToArray();

				var heretic = false;
				foreach (var overwrite in roleOverwrites)
				{
					var owRole = overwrite.GetRoleAsync().Result;
					var owPermission = overwrite.CheckPermission(Permissions.AccessChannels);

					if (owRole.Id == crusaderRole?.Id && owPermission == PermissionLevel.Allowed ||
					    owRole.Id == purifierRole?.Id && owPermission == PermissionLevel.Allowed
					) //crusaders are granted access, not heretic channel
					{
						heretic = false;
						break;
					}

					if (!(owRole.Id == hereticRole?.Id && owPermission == PermissionLevel.Allowed ||
					      owRole.Id == changedRole?.Id && owPermission == PermissionLevel.Allowed)
					) //current role override not for heretics, skip to next
					{
						continue;
					}

					heretic = true;
				}

				if (heretic)
					hereticChannels.Add(channel);
			}

			var authorMember = crusaderMembers.FirstOrDefault(member => member.Id == e.Author.Id);
			if (authorMember?.Id == e.Author.Id && hereticChannels.Contains(e.Channel))
			{
				await e.Message.DeleteAsync(":MagicFox:");
				var e621 = new E621Client();
				var urls = e621.GetRandomPostsUrl("reaction_image rating:s", 10, 10).Result;

				if (urls.Count > 0)
				{
					var responseTask =
						e.Message.RespondAsync(
							"Bad human, this channel is not meant for you. Have some heresy as punishment!");
					var heresyTask = authorMember.SendMessageAsync(string.Join('\n', urls));
					Task.WaitAll(responseTask, heresyTask);
					await Task.Delay(5000);
					await responseTask.Result.DeleteAsync();
				}
			}
		}
	}
}