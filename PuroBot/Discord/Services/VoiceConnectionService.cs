using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace PuroBot.Discord.Services
{
	public class VoiceConnectionService
	{
		private static readonly TimeSpan ChannelTimeout = new(0, 5, 0);

		private readonly List<ConnectionInfo> _activeConnections = new();

		public async Task<VoiceInfo> AcquireChannel(ICommandContext context, IVoiceChannel channel = null)
		{
			// join the specified/default voice channel
			// 
			// if already connected to a different voice channel -> wait until no longer in use, rejoin to new channel
			// if same channel -> wait until no longer in use before return
			// if not connected -> join and lock

			var connection = GetActiveConnection(context.Guild.Id);

			if (connection is null)
			{
				// not connected -> connect and lock
				return await CreateNewConnection(context, channel);
			}

			var targetChannel = await GetTargetChannel(context, channel ?? connection.VoiceInfo.VoiceChannel);
			if (targetChannel is null)
			{
				return null;
			}

			await connection.Lock();
			
			if (connection.VoiceInfo.VoiceChannel != targetChannel)
			{
				// target and current different, rejoin
				connection.VoiceInfo = await JoinChannel(targetChannel);
			}

			return connection.VoiceInfo;
		}

		public void ReleaseChannel(ICommandContext context)
		{
			// start timeout for disconnect and unlock
			//
			// if not connected -> throw
			// if channel not locked -> throw
			var connection = GetActiveConnection(context.Guild.Id);

			if (connection is null || !connection.IsLocked)
				throw new ArgumentException("no locked connection to release", nameof(context));

			connection.Release();
		}

		public Task LeaveChannel(ICommandContext context)
		{
			RemoveConnection(context.Guild.Id);
			return Task.CompletedTask;
		}

		private async Task<VoiceInfo> CreateNewConnection(ICommandContext context, IVoiceChannel channel)
		{
			var targetChannel = await GetTargetChannel(context, channel);
			var voiceInfo = await JoinChannel(targetChannel);
			if (voiceInfo is null)
			{
				// connection failed, do not lock
				return null;
			}

			var connection = new ConnectionInfo(context.Guild.Id, voiceInfo, () => RemoveConnection(context.Guild.Id));
			await connection.Lock();

			AddConnection(connection);

			return connection.VoiceInfo;
		}

		private static async Task<IVoiceChannel> GetTargetChannel(ICommandContext context, IVoiceChannel channel)
		{
			var targetChannel = channel;

			// if no channel specified, use channel of user
			targetChannel ??= (context.User as IGuildUser)?.VoiceChannel;

			if (targetChannel is not null) return targetChannel;

			// target channel still unknown, can't connect
			await context.Channel.SendMessageAsync(
				"User must be in a voice channel, or a voice channel must be passed as an argument.");
			return null;
		}

		private static async Task<VoiceInfo> JoinChannel(IVoiceChannel channel)
		{
			var audioClient = await channel.ConnectAsync();
			var audioStream = audioClient.CreatePCMStream(AudioApplication.Mixed, bufferMillis: 200);
			var voiceInfo = new VoiceInfo(audioStream, channel);

			return voiceInfo;
		}

		private void AddConnection(ConnectionInfo connection)
		{
			lock (_activeConnections)
			{
				_activeConnections.Add(connection);
			}
		}

		private ConnectionInfo GetActiveConnection(ulong guildId)
		{
			lock (_activeConnections)
			{
				return _activeConnections.FirstOrDefault(i => i.GuildId == guildId);
			}
		}

		private void RemoveConnection(ulong guildId)
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
			private readonly Timer _timeoutTimer;
			private readonly SemaphoreSlim _usageLock = new(1);
			private VoiceInfo _voiceInfo;

			public ConnectionInfo(ulong guildId, VoiceInfo voiceInfo, Action timeoutAction)
			{
				GuildId = guildId;
				_voiceInfo = voiceInfo;

				_timeoutTimer = new Timer(_ => timeoutAction());
			}

			public ulong GuildId { get; }

			public VoiceInfo VoiceInfo
			{
				get => _voiceInfo;
				set
				{
					_voiceInfo?.Dispose();
					_voiceInfo = value;
				}
			}

			public bool IsLocked { get; private set; }

			public void Dispose()
			{
				_voiceInfo?.Dispose();
				_timeoutTimer?.Dispose();
				_usageLock?.Dispose();
			}

			public async Task Lock()
			{
				await _usageLock.WaitAsync();
				IsLocked = true;
				StopTimeout();
			}

			public void Release()
			{
				_usageLock.Release();
				IsLocked = false;
				StartTimeout();
			}

			private void StartTimeout() => _timeoutTimer.Change(ChannelTimeout, Timeout.InfiniteTimeSpan);

			private void StopTimeout() => _timeoutTimer.Change(Timeout.Infinite, Timeout.Infinite);
		}
	}
}