using ToolBox.Injection;
using ToolBox.Services;

namespace ToolBox.Debug
{
	public class LogSettings : IService
	{
		public LogLevel allowedLogs = LogLevel.All;
    }

	public class ToolBoxLogger : NamedUnityLogger, IService
	{
        public ToolBoxLogger() : base("ToolBox")
        {

        }
    }

    public class NamedUnityLogger : UnityLogger
	{
		public string Nametag { get; private set; }

        public NamedUnityLogger(string nametag)
        {
	        this.Nametag = string.Intern(DebugSymbolConstants.FormatNametag(nametag) + " ");
        }

        protected override void Log(LogLevel level, string message)
        {
	        base.Log(level,  Nametag + " "+ message);
        }
    }
}