namespace ToolBox.Debug
{
    public interface ILogger
    {
	    void DeepInfo(string message);
		void Info(string message);
		void Warn(string message);
		void Error(string message);
		void Assert(string message);
		void Fatal(string message);
	}
}