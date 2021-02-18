using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace PuroBot.Extensions
{
	public static class DiscordExtensions
	{
		public static async Task SendMany(this IEnumerable<string> messages,
			Func<string, Task<IUserMessage>> sendMsgFunc)
		{
			var msgChunks = messages.Partition();
			foreach (var chunk in msgChunks)
			{
				var msg = string.Join('\n', chunk);
				await sendMsgFunc.Invoke(msg);
			}
		}

		public static string DecodePrecondition(this PreconditionAttribute precondition)
		{
			switch (precondition)
			{
				case RequireNsfwAttribute _:
					return "NSFW channel";
				case RequireUserPermissionAttribute attr:
					var decode = attr.ChannelPermission.HasValue
						? $"Channel Permission {attr.ChannelPermission.ToString().AsCode()}"
						: string.Empty;
					decode += (string.IsNullOrWhiteSpace(decode) ? string.Empty : ", ") + (attr.GuildPermission.HasValue
						? $"Guild Permission {attr.GuildPermission.ToString().AsCode()}"
						: string.Empty);
					return decode;
				default:
					return precondition.GetType().Name;
			}
		}
	}
}