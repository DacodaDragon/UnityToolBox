using System;
using System.Collections.Generic;

namespace ToolBox
{
	public static class ListExtensions
	{
		public static bool TryFindFirst<T>(this List<T> list, Func<T, bool> predicate, out T value)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (predicate.Invoke(list[i]))
				{
					value = list[i];
					return true;
				}
			}

			value = default(T);
			return false;
		}
	}
}
