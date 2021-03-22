using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolBox
{
    public static class TypeHelper 
	{
		public static string[] GetFieldNames(object obj)
        {
			return obj.GetType().GetFields().Select(x => x.Name).ToArray();
		}

		public static Type FindTypeByFullName(string fullName)
        {
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				var type = assembly
					.GetTypes()
					.FirstOrDefault(x => x.FullName == fullName);

				if (type != null)
					return type;
			}

			return null;
		}

		public static Type[] GetAllTypesThatInherit(Type type)
        {
			List<Type> types = new List<Type>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies)
			{
				types.AddRange(assembly.GetTypes().Where(
					x => type.IsAssignableFrom(x) &&
						type != x.GetType()));
			}

			return types.ToArray();
		}

		public static Type[] GetAllTypesThatInherit<T>()
		{
			return GetAllTypesThatInherit(typeof(T));
		}
	}
}

