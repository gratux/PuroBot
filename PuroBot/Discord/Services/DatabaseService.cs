using Microsoft.EntityFrameworkCore;
using PuroBot.Common;
using PuroBot.Models;
using PuroBot.Models.Domain;

namespace PuroBot.Discord.Services
{
	public class DatabaseService
	{
		private readonly DatabaseContext _dbContext = new();

		/// <summary>
		///     Saves all changes to the database
		/// </summary>
		/// <returns><see langword="true" /> if all changes were saved successfully; <see langword="false" /> otherwise</returns>
		private bool SaveDatabase()
		{
			try
			{
				_dbContext.SaveChanges();
				return true;
			}
			catch (DbUpdateException exception)
			{
				Logging.Error("Failed to save Database", exception);
				return false;
			}
		}

		public string GetServerCommandPrefix(ulong serverId)
		{
			ServerSettings? settings = _dbContext.Servers?.Find(serverId);
			if (settings is not null)
				return settings.Prefix;

			settings = new ServerSettings(serverId);
			_dbContext.Servers?.Add(settings);
			SaveDatabase();

			return settings.Prefix;
		}

		public bool SetServerCommandPrefix(ulong serverId, string newPrefix)
		{
			ServerSettings? settings = _dbContext.Servers?.Find(serverId);
			if (settings is null)
			{
				settings = new ServerSettings(serverId);
				_dbContext.Servers?.Add(settings);
			}

			settings.Prefix = newPrefix;
			return SaveDatabase();
		}
	}
}