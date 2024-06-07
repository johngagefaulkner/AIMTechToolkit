using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMTechToolkit
{
	internal static class Log
	{
		private static bool IsInitialized { get; set; } = false;

		private const string LogFileName = "AIMTechToolkit_MainLog.log";
		private static string LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AIMTechToolkit", LogFileName);

		public static void Initialize()
		{
			if (!IsInitialized)
			{
				if (!Directory.Exists(Path.GetDirectoryName(LogFilePath)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
				}

				if (!File.Exists(LogFilePath))
				{
					File.WriteAllText(LogFilePath, $"AIMTechToolkit Main Log\n\n");
				}

				IsInitialized = true;
			}
		}

		private static void AppendLogEntryToFile(string _logMessage, string _logSeverity)
		{
			if (IsInitialized is false)
			{
				System.Diagnostics.Debug.WriteLine("Log service not initialized. Cannot log message.");
				return;
			}

			var logSeverity = $"[{_logSeverity}]";
			var logTimeStamp = $"[{DateTime.Now.ToString("yyyy-MM-dd")}] [{DateTime.Now.ToString("hh:mm:ss")}]";
			File.AppendAllText(LogFilePath, $"{logTimeStamp} {logSeverity} {_logMessage}\n");
		}

		internal static void Info(string LogMessage) => AppendLogEntryToFile(LogMessage, "INFO");
		internal static void Warning(string LogMessage) => AppendLogEntryToFile(LogMessage, "WARN");
		internal static void Error(string LogMessage) => AppendLogEntryToFile(LogMessage, "ERROR");
		internal static void Error(Exception LogException) => AppendLogEntryToFile(LogException.ToString(), "ERROR");
		internal static void Error(Exception LogException, string LogMessage)
		{
			StringBuilder sb = new();
			sb.AppendLine(LogMessage);
			sb.AppendLine($"Error Message: {LogException.Message}");
			sb.AppendLine($"StackTrace: {LogException.StackTrace}");
			sb.AppendLine($"Full Exception: {LogException.ToString()}");
			AppendLogEntryToFile(sb.ToString(), "ERROR");
		}
		internal static void Debug(string LogMessage) => AppendLogEntryToFile(LogMessage, "DEBUG");
		internal static void Verbose(string LogMessage) => AppendLogEntryToFile(LogMessage, "VERBOSE");
	}
}
