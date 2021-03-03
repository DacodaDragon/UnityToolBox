using System;

namespace ToolBox.Injection
{
    public class GenericObjectFactory
	{
		public static T CreateObject<T>(Injector injector)
		{
			T instance = Activator.CreateInstance<T>();
			injector.InjectDependencies(instance);
			return instance;
		}

		public static object CreateObject(Type type, Injector injector)
		{
			object instance = Activator.CreateInstance(type);
			if (injector != null)
				injector.InjectDependencies(instance);
			return instance;
		}
	}
}
