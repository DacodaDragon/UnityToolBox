using System.Collections.Generic;
using System;
using ToolBox.Services;

namespace ToolBox.Injection
{
    class ServiceProvider : DependencyInstanceProvider
    {
        private Dictionary<Type, IService> _Cache = new Dictionary<Type, IService>();

        override protected bool RequestType(Type type, out object result)
        {
            if (!typeof(IService).IsAssignableFrom(type))
            {
                result = null;
                return false;
            }

            if (_Cache.TryGetValue(type, out IService service))
            {
                result = service;
                return true;
            }

            result = GenericObjectFactory.CreateObject(type, currentInjector);
            return true;
        }
    }
}
