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
		public class VoiceInfo : IDisposable
		{
			public VoiceInfo(AudioOutStream audioStream, IVoiceChannel voiceChannel)
			{
				AudioStream = audioStream;
				VoiceChannel = voiceChannel;
			}

			public AudioOutStream AudioStream { get; }
			public IVoiceChannel VoiceChannel { get; }

			public void Dispose()
			{
				AudioStream?.Dispose();
			}
		}

		private class ConnectionInfo : IDisposable
		{
			public ConnectionInfo(ulong guildId, VoiceInfo voiceInfo)
			{
				GuildId = guildId;
				VoiceInfo = voiceInfo;
			}

			public void UpdateTime()
			{
				LastUsed = DateTime.UtcNow;
			}

			public ulong GuildId { get; }
			public DateTime LastUsed { get; private set; }

			private VoiceInfo _voiceInfo;

			public VoiceInfo VoiceInfo
			{
				get => _voiceInfo;
				set
				{
					_voiceInfo?.Dispose();
					_voiceInfo = value;
					UpdateTime();
				}
			}

			public void Dispose()
			{
				_voiceInfo?.Dispose();
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
			// no special channel requested, use channel of calling user 
			channel ??= (context.User as IGuildUser)?.VoiceChannel;

			// user not in any channel and no channel specified
			if (channel == null)
			{
				await context.Channel.SendMessageAsync(
					"User must be in a voice channel, or a voice channel must be passed as an argument.");
				return null;
			}

			// look for entries for current server
			var voiceInfo = GetInfo(context.Guild.Id);
			// server not in list or different channel requested
			if (voiceInfo == null || voiceInfo.VoiceChannel != channel)
			{
				var audioStream =
					(await channel.ConnectAsync()).CreatePCMStream(AudioApplication.Mixed, bufferMillis: 200);
				voiceInfo = new VoiceInfo(audioStream, channel);
				Add(context.Guild.Id, voiceInfo);
			}
			// already connected -> only update time
			else
			{
				Update(context.Guild.Id, voiceInfo);
			}

			return voiceInfo;
		}

		public Task LeaveChannel(ICommandContext context)
		{
			GetInfo(context.Guild.Id)?.VoiceChannel.DisconnectAsync();
			Remove(context.Guild.Id);
			return Task.CompletedTask;
		}

		private void Add(ulong guildId, VoiceInfo channel)
		{
			lock (_activeConnections)
			{
				_activeConnections.Add(new ConnectionInfo(guildId, channel));
			}
		}

		private void Update(ulong guildId, VoiceInfo channel)
		{
			lock (_activeConnections)
			{
				var connection = _activeConnections.First(i => i.GuildId == guildId);

				// different channel -> reconnect to new channel
				if (connection.VoiceInfo != channel)
					connection.VoiceInfo = channel;
				// same channel -> update last used
				else
					connection.UpdateTime();
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
				foreach (var info in _activeConnections.Where(i => i.GuildId == guildId))
				{
					info.Dispose();
				}

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