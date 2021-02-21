using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace PuroBot.Services
{
	public class VoiceConnectionService
	{
		private static readonly TimeSpan Timeout = new(0, 5, 0);
		private readonly List<ConnectionInfo> _activeConnections = new();

		/// <summary>
		///     if already connected to a voice channel of the same guild, reuse it
		///     if not connected in this guild or a voice channel is specified, create a new connection
		/// </summary>
		/// <param name="context">the command context</param>
		/// <param name="channel">the voice channel to be used</param>
		/// <returns>information about the now connected voice channel</returns>
		public async Task<VoiceInfo> JoinOrReuseChannel(ICommandContext context, IVoiceChannel channel = null)
		{
			// look for entries for current server
			var voiceInfo = GetInfo(context.Guild.Id);

			// if no channel is specified, reuse current channel
			channel ??= voiceInfo?.VoiceChannel;

			// not connected to any channel, use channel of calling user 
			channel ??= (context.User as IGuildUser)?.VoiceChannel;

			// target channel still unknown, can't connect
			if (channel is null)
			{
				await context.Channel.SendMessageAsync(
					"User must be in a voice channel, or a voice channel must be passed as an argument.");
				return null;
			}

			// server not in list or different channel requested
			if (voiceInfo is null || voiceInfo.VoiceChannel != channel)
			{
				var audioClient = await channel.ConnectAsync();
				var audioStream = audioClient.CreatePCMStream(AudioApplication.Mixed, bufferMillis: 200);
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
			Remove(context.Guild.Id);
			return Task.CompletedTask;
		}

		private void Add(ulong guildId, VoiceInfo channel)
		{
			lock (_activeConnections)
			{
				_activeConnections.Add(new ConnectionInfo(guildId, channel, () => Remove(guildId)));
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
				// same channel -> update timeout
				else
					connection.UpdateTimeout();
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
				foreach (var info in _activeConnections.Where(i => i.GuildId == guildId)) info.Dispose();

				_activeConnections.RemoveAll(i => i.GuildId == guildId);
			}
		}

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
				VoiceChannel?.DisconnectAsync();
				AudioStream?.Dispose();
				GC.SuppressFinalize(this);
			}
		}

		private class ConnectionInfo : IDisposable
		{
			private readonly Action _timeoutAction;
			private Timer _timeoutTimer;

			private VoiceInfo _voiceInfo;

			public ConnectionInfo(ulong guildId, VoiceInfo voiceInfo, Action timeoutAction)
			{
				GuildId = guildId;
				VoiceInfo = voiceInfo;
				_timeoutAction = timeoutAction;

				// UpdateTimeout();
			}

			public ulong GuildId { get; }

			public VoiceInfo VoiceInfo
			{
				get => _voiceInfo;
				set
				{
					_voiceInfo?.Dispose();
					_voiceInfo = value;
					UpdateTimeout();
				}
			}

			public void Dispose()
			{
				_timeoutTimer?.Dispose();
				_voiceInfo?.Dispose();
			}

			public void UpdateTimeout()
			{
				_timeoutTimer?.Stop();
				_timeoutTimer?.Dispose();

				_timeoutTimer = new Timer
				{
					Interval = Timeout.TotalMilliseconds,
					AutoReset = false,
					Enabled = true
				};
				_timeoutTimer.Elapsed += (_, _) => _timeoutAction.Invoke();
			}
		}
	}
}