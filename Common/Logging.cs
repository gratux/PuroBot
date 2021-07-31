using System;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Config;
using log4net.Core;

namespace PuroBot.Common
{
	public static class Logging
	{
		private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

		static Logging()
		{
			var configurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "log4net.config");
			XmlConfigurator.Configure(new FileInfo(configurationPath));
		}

		private static string BuildMessage(string msg, string source)
		{
			var msgBuilder = new StringBuilder();

			msgBuilder.AppendFormat("{0}: ", source);

			if (!string.IsNullOrWhiteSpace(msg))
				msgBuilder.Append(msg);

			return msgBuilder.ToString();
		}

		/// <summary>
		///     Log a <see cref="Level.Fatal" /> message to the default log output
		/// </summary>
		/// <param name="msg">the message to be logged</param>
		/// <param name="exception">the <see cref="Exception" /> that has occured (optional)</param>
		/// <param name="source">where the <paramref name="msg" /> originated (optional)</param>
		public static void Fatal(string msg, Exception? exception = null, string source = "PuroBot") =>
			Logger.Fatal(BuildMessage(msg, source), exception);

		/// <summary>
		///     Log a <see cref="Level.Error" /> message to the default log output
		/// </summary>
		/// <param name="msg">the message to be logged</param>
		/// <param name="exception">the <see cref="Exception" /> that has occured (optional)</param>
		/// <param name="source">where the <paramref name="msg" /> originated (optional)</param>
		public static void Error(string msg, Exception? exception = null, string source = "PuroBot") =>
			Logger.Error(BuildMessage(msg, source), exception);

		/// <summary>
		///     Log a <see cref="Level.Warn" /> message to the default log output
		/// </summary>
		/// <param name="msg">the message to be logged</param>
		/// <param name="exception">the <see cref="Exception" /> that has occured (optional)</param>
		/// <param name="source">where the <paramref name="msg" /> originated (optional)</param>
		public static void Warn(string msg, Exception? exception = null, string source = "PuroBot") =>
			Logger.Warn(BuildMessage(msg, source), exception);

		/// <summary>
		///     Log a <see cref="Level.Info" /> message to the default log output
		/// </summary>
		/// <param name="msg">the message to be logged</param>
		/// <param name="exception">the <see cref="Exception" /> that has occured (optional)</param>
		/// <param name="source">where the <paramref name="msg" /> originated (optional)</param>
		public static void Info(string msg, Exception? exception = null, string source = "PuroBot") =>
			Logger.Info(BuildMessage(msg, source), exception);

		/// <summary>
		///     Log a <see cref="Level.Debug" /> message to the default log output
		/// </summary>
		/// <param name="msg">the message to be logged</param>
		/// <param name="exception">the <see cref="Exception" /> that has occured (optional)</param>
		/// <param name="source">where the <paramref name="msg" /> originated (optional)</param>
		public static void Debug(string msg, Exception? exception = null, string source = "PuroBot") =>
			Logger.Debug(BuildMessage(msg, source), exception);
	}
}