using System;
using UnityEngine;

namespace ToolBox.Debug
{
    [Flags]
	public enum LogLevel
	{
		None		= 1 << 0,
		All			= DeepInfo | Info | Warning | Error | Assertion | Fatal,

		DeepInfo	= 1 << 1,
		Info		= 1 << 2,
        Warning		= 1 << 3,
		Error		= 1 << 4,
		Assertion	= 1 << 5,
		Fatal		= 1 << 6
	}

	public static class LogLevelExtensions
	{
		public static LogType ToUnityLogType(this LogLevel level)
		{
            switch (level)
            {
                case LogLevel.Info:
                case LogLevel.DeepInfo:
	                return LogType.Log;
                case LogLevel.Warning:
	                return LogType.Warning;
                case LogLevel.Error:
                case LogLevel.Fatal:
	                return LogType.Error;
                case LogLevel.Assertion:
	                return LogType.Assert;
                default:
	                const string message = "LogLevel of level ({0}) cannot be converted to LogType.\n" +
	                                       "Please make sure a single flag is set (none or multiple not allowed).";
	                throw new InvalidCastException(string.Format(message, level));
            }
        }

		public static LogLevel ToLogLevel(this LogType type)
		{
            switch (type)
            {
                case LogType.Error: return LogLevel.Error;
                case LogType.Assert: return LogLevel.Assertion;
                case LogType.Warning: return LogLevel.Warning;
                case LogType.Log: return LogLevel.Info;
                case LogType.Exception: return LogLevel.Fatal;
                default:
                    const string message = "Invalid LogType provided during cast: {0}";
                    throw new InvalidCastException(string.Format(message, type));
            }
        }
    }
}