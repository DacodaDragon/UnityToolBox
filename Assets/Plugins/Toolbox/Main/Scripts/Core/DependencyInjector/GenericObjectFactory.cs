using System;
using UnityEngine;

namespace ToolBox.Injection
{
    public class GenericObjectFactory
	{
		public static object CreateObject(string typeName, Injector injector, string jsonData = null)
        {
			Type type = TypeHelper.FindTypeByFullName(typeName);
			return CreateObject(type, injector, jsonData);
        }

		public static T CreateObject<T>(Injector injector)
		{
			T instance = Activator.CreateInstance<T>();
			injector.InjectDependencies(instance);
			return instance;
		}

		public static object CreateObject(Type type, Injector injector, string jsonData = null)
		{
			object instance;

			if (string.IsNullOrEmpty(jsonData))
            {
                instance = Activator.CreateInstance(type);
            }
            else
			{
                try
                {
					instance = JsonUtility.FromJson(jsonData, type);
                }
                catch (Exception e)
                {
					instance = Activator.CreateInstance(type);
                }
			}

			if (injector != null)
				injector.InjectDependencies(instance);
			return instance;
		}
	}
}
