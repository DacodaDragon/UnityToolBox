using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolBox
{
	public static class TypeHelper 
	{
		public static Type[] GetAllTypesThatInherit<T>()
		{
			List<Type> types = new List<Type>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies)
			{
				types.AddRange(assembly.GetTypes().Where(
					x=> typeof(T).IsAssignableFrom(x) &&
					    typeof(T) != x.GetType()));
			}

			return types.ToArray();
		}
	}
}

