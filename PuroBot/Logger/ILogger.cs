using System;
using System.Runtime.CompilerServices;

namespace PuroBot.Logger
{
	public interface ILogger
	{
		void Log(string msg, [CallerMemberName] string src = "unspecified");
		void LogWarning(string msg, [CallerMemberName] string src = "unspecified");
		void LogError(string msg, [CallerMemberName] string src = "unspecified");
		void LogError(Exception ex, [CallerMemberName] string src = "unspecified");
		void LogDebug(string msg, [CallerMemberName] string src = "unspecified");
	}
}