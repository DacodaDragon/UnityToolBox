using System.Diagnostics;

namespace ToolBox.Debug
{
    public abstract class UnityLogger : ILogger
    {
		[DebuggerStepThrough]
        public void Assert(string message)
        {
	        Log(LogLevel.Assertion, message);
        }

		[DebuggerStepThrough]
        public void DeepInfo(string message)
        {
	        Log(LogLevel.DeepInfo, message);
        }

		[DebuggerStepThrough]
        public void Error(string message)
        {
	        Log(LogLevel.Error, message);
        }

		[DebuggerStepThrough]
        public void Fatal(string message)
        {
	        Log(LogLevel.Fatal, message);
        }

		[DebuggerStepThrough]
        public void Info(string message)
        {
	        Log(LogLevel.Info, message);
        }

		[DebuggerStepThrough]
        public void Warn(string message)
        {
	        Log(LogLevel.Warning, message);
        }

		[DebuggerStepThrough]
        protected virtual void Log(LogLevel level, string message)
        {
	        UnityEngine.Debug.unityLogger.Log(level.ToUnityLogType(), message);
        }
    }
}