using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PuroBot.Logger
{
	public class DefaultLogger : ILogger
	{
		private static readonly SemaphoreSlim Semaphore = new(1);

		public void Log(string msg, [CallerMemberName] string src = "unspecified")
		{
			Task.Run(() => PrintLogMessage(msg, new LogHeader(ConsoleColor.Cyan, "Info", src)));
		}

		public void LogWarning(string msg, [CallerMemberName] string src = "unspecified")
		{
			Task.Run(() => PrintLogMessage(msg, new LogHeader(ConsoleColor.Yellow, "Warning", src)));
		}

		public void LogError(string msg, [CallerMemberName] string src = "unspecified")
		{
			Task.Run(() => PrintLogMessage(msg, new LogHeader(ConsoleColor.Red, "Error", src)));
		}

		public void LogError(Exception ex, [CallerMemberName] string src = "unspecified")
		{
			LogError(ex.ToString(), src);
		}

		public void LogDebug(string msg, [CallerMemberName] string src = "unspecified")
		{
			Task.Run(() => PrintLogMessage(msg, new LogHeader(ConsoleColor.Magenta, "Debug", src)));
		}

		private static void PrintLogMessage(string msg, LogHeader header)
		{
			Semaphore.Wait();
			WriteHeader(header);
			Console.WriteLine(msg);
			Semaphore.Release();
		}

		private static void WriteHeader(LogHeader header)
		{
			var time = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture);
			Console.Write($"[{time}\\");
			Console.ForegroundColor = header.Color;
			Console.Write($"{header.Level.PadRight(7)}");
			Console.ResetColor();
			Console.Write($" {header.Source}] - ");
		}

		private class LogHeader
		{
			public LogHeader(ConsoleColor color, string level, string source)
			{
				Color = color;
				Level = level;
				Source = source;
			}

			public ConsoleColor Color { get; }
			public string Level { get; }
			public string Source { get; }
		}
	}
}