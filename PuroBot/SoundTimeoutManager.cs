using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace PuroBot
{
	public class SoundTimeoutManager
	{
		private readonly DiscordClient _client;
		private readonly Dictionary<DiscordGuild, DateTime> _lastUsed = new Dictionary<DiscordGuild, DateTime>();
		private readonly TimeSpan _timeout = new TimeSpan(0, 5, 0);

		public SoundTimeoutManager(DiscordClient client)
		{
			_client = client;
			var checkTimeoutThread = new Thread(CheckTimeout) {IsBackground = true};
			checkTimeoutThread.Start();
		}

		public void AddOrUpdate(DiscordGuild guild)
		{
			lock (_lastUsed)
			{
				if (_lastUsed.ContainsKey(guild))
					_lastUsed[guild] = DateTime.UtcNow;
				else
					_lastUsed.Add(guild, DateTime.UtcNow);
			}
		}

		public void Remove(DiscordGuild guild)
		{
			lock (_lastUsed)
			{
				if (_lastUsed.ContainsKey(guild))
					_lastUsed.Remove(guild);
			}
		}

		[SuppressMessage("ReSharper", "FunctionNeverReturns")]
		private void CheckTimeout()
		{
			while (true)
			{
				DiscordGuild[] lastUsedCopy;
				lock (_lastUsed)
				{
					lastUsedCopy = _lastUsed
						.Where(p => p.Value + _timeout <= DateTime.UtcNow)
						.Select(p => p.Key).ToArray();
				}

				foreach (var guild in lastUsedCopy)
				{
					Disconnect(guild);
					lock (_lastUsed)
					{
						_lastUsed.Remove(guild);
					}
				}
			}
		}

		private void Disconnect(DiscordGuild guild)
		{
			var vnext = _client.GetVoiceNext();
			var connection = vnext.GetConnection(guild);
			connection.Disconnect();
		}
	}
}