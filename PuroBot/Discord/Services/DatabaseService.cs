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

		/// <summary>
		///     gets the server command prefix for a given server
		/// </summary>
		/// <remarks>if the server does not have a database entry, a new one will be created</remarks>
		/// <param name="serverId">the id of the server</param>
		/// <returns>the command prefix</returns>
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

		/// <summary>
		///     sets a custom command prefix for a given server
		/// </summary>
		/// <param name="serverId">the id of the server</param>
		/// <param name="newPrefix">the new command prefix</param>
		/// <returns><see langword="true" /> if the prefix was saved successfully; <see langword="false" /> otherwise</returns>
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