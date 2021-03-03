namespace ToolBox.DataHolders
{
	public struct IntRange
	{
		/// <summary>
		/// From index, inclusive
		/// </summary>
		public readonly int From;

		/// <summary>
		/// To index, exclusive
		/// </summary>
		public readonly int To;

		public bool contains(int x)
		{
			return x >= From && x < To;
		}

		public IntRange(int from, int to)
		{
			From = from;
			To = to;
		}
	}
}
