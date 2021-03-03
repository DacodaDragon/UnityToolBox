namespace ToolBox
{
	public interface IReservable
	{
		bool IsReserved { get; }
		void Reserve();
		void UnReserve();
	}
}
