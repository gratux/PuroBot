using System.IO;
using Microsoft.Extensions.Configuration;

namespace PuroBot.Common
{
	/// <summary>
	///     holds all application secrets
	/// </summary>
	public static class Secrets
	{
		private static readonly IConfiguration AppSettings;

		static Secrets() =>
			AppSettings = new ConfigurationBuilder()
				.AddJsonFile(Path.Combine("Config", "appSecrets.json"), false, true)
				.Build();

		/// <summary>
		///     the connection string to the database
		/// </summary>
		public static string DbConnectionString => AppSettings.GetConnectionString("PostgreSql");

		/// <summary>
		///     the Discord Bot Token used for interfacing with the API
		/// </summary>
		public static string BotToken => AppSettings.GetSection("ApiKeys")["Discord"];
	}
}