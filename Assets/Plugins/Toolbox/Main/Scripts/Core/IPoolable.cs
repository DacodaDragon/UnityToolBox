namespace ToolBox.Collections
{
	public interface IPoolable
	{
		bool IsPooled { get; }
		void UnPool();
		void Pool();
	}
}
