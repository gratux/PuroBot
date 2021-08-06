using System.IO;
using Microsoft.Extensions.Configuration;

namespace PuroBot.Common
{
    public static class Settings
    {
        static Settings()
        {
            AppSettings = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine("Config", "appsettings.json"), false, true)
                .Build();
        }

        /// <summary> the name of the default <see cref="log4net.ILog" /> </summary>
        public static string LoggerName => AppSettings.GetSection("Logger")["Name"];

        /// <summary> the default command prefix </summary>
        public static string DefaultPrefix => "~";

        private static readonly IConfiguration AppSettings;
    }
}
