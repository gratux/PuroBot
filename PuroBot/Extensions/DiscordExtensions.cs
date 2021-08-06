using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace PuroBot.Extensions
{
    public static class DiscordExtensions
    {
        public static string DecodePrecondition(this PreconditionAttribute precondition)
        {
            switch (precondition)
            {
                case RequireNsfwAttribute:
                    return "NSFW channel";
                case RequireUserPermissionAttribute attr:
                {
                    string decode = attr.ChannelPermission.HasValue
                        ? $"Channel Permission {attr.ChannelPermission.ToString()?.AsCode()}"
                        : string.Empty;
                    decode += (string.IsNullOrWhiteSpace(decode) ? string.Empty : ", ") + (attr.GuildPermission.HasValue
                        ? $"Guild Permission {attr.GuildPermission.ToString()?.AsCode()}"
                        : string.Empty);
                    return decode;
                }
                case RequireContextAttribute attr:
                    return string.Join(" or ", attr.Contexts.ToString("G").Split(", ")) + " Context";

                default:
                    return precondition.GetType().Name;
            }
        }

        public static async Task SendMany(this IAsyncEnumerable<string?> messages,
            Func<string, Task<IUserMessage>> sendMsgFunc)
        {
            var msgChunks = messages.Partition();
            await foreach (var chunk in msgChunks)
            {
                string msg = string.Join('\n', chunk);
                await sendMsgFunc.Invoke(msg);
            }
        }

        public static async Task SendTemporary(string msg,
            TimeSpan timeout,
            Func<string, Task<IUserMessage>> sendMsgFunc)
        {
            var responseTask = sendMsgFunc.Invoke(msg);
            await responseTask;
            await Task.Delay(timeout);
            await responseTask.Result.DeleteAsync();
        }
    }
}
