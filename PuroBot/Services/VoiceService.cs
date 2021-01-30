using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace PuroBot.Services
{
	public class VoiceService
	{
		public class VoiceInfo
		{
			public VoiceInfo(IAudioClient audioClient, IVoiceChannel voiceChannel)
			{
				AudioClient = audioClient;
				VoiceChannel = voiceChannel;
			}

			public IAudioClient AudioClient { get; }
			public IVoiceChannel VoiceChannel { get; }
		}
		private class ConnectionInfo
		{
			public ConnectionInfo(ulong guildId, VoiceInfo voiceInfo)
			{
				GuildId = guildId;
				VoiceInfo = voiceInfo;
			}

			public ulong GuildId { get; }
			public DateTime LastUsed { get; private set; }

			private VoiceInfo _voiceInfo;

			public VoiceInfo VoiceInfo
			{
				get => _voiceInfo;
				set
				{
					_voiceInfo = value;
					LastUsed = DateTime.UtcNow;
				}
			}
		}

		private readonly List<ConnectionInfo> _activeConnections = new List<ConnectionInfo>();
		private readonly TimeSpan _timeout = new TimeSpan(0, 5, 0);

		public VoiceService()
		{
			var checkTimeoutThread = new Thread(CheckTimeout) {IsBackground = true};
			checkTimeoutThread.Start();
		}
		
		public async Task<VoiceInfo> JoinChannel(ICommandContext context, IVoiceChannel channel = null)
		{
			channel ??= (context.User as IGuildUser)?.VoiceChannel;
			
			if (channel == null)
			{
				await context.Channel.SendMessageAsync(
					"User must be in a voice channel, or a voice channel must be passed as an argument.");
				return null;
			}

			//don't rejoin same channel
			var voiceInfo = GetInfo(context.Guild.Id);
			if (voiceInfo?.VoiceChannel == channel)
				return voiceInfo;
			
			voiceInfo = new VoiceInfo(await channel.ConnectAsync(), channel);
			AddOrUpdate(context.Guild.Id, voiceInfo);
			return voiceInfo;
		}

		public async Task LeaveChannel(ICommandContext context)
		{
			GetInfo(context.Guild.Id)?.VoiceChannel.DisconnectAsync();
			Remove(context.Guild.Id);
		}

		private void AddOrUpdate(ulong guildId, VoiceInfo channel)
		{
			lock (_activeConnections)
			{
				if (_activeConnections.All(i => i.GuildId != guildId))
					_activeConnections.Add(new ConnectionInfo(guildId, channel));
				else
					_activeConnections.First(i => i.GuildId == guildId).VoiceInfo = channel;
			}
		}

		private VoiceInfo GetInfo(ulong guildId)
		{
			lock (_activeConnections)
			{
				return _activeConnections.FirstOrDefault(i => i.GuildId == guildId)?.VoiceInfo;
			}
		}

		private void Remove(ulong guildId)
		{
			lock (_activeConnections)
			{
				_activeConnections.RemoveAll(i => i.GuildId == guildId);
			}
		}

		[SuppressMessage("ReSharper", "FunctionNeverReturns")]
		private void CheckTimeout()
		{
			while (true)
			{
				ConnectionInfo[] oldConnections;
				lock (_activeConnections)
				{
					oldConnections = _activeConnections
						.Where(p => p.LastUsed + _timeout <= DateTime.UtcNow).ToArray();
				}

				foreach (var connection in oldConnections)
				{
					connection.VoiceInfo.VoiceChannel.DisconnectAsync();
					Remove(connection.GuildId);
				}
			}
		}
	}
}