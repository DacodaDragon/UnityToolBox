using System.Collections.Generic;
using System;

namespace ToolBox.Injection
{
    class ServiceProvider : DependencyInstanceProvider
    {
        private Dictionary<Type, object> _Cache = new Dictionary<Type, object>();

        override protected bool RequestType(Type type, out object result)
        {
            if (!type.IsAssignableFrom(typeof(IService)))
            {
                result = null;
                return false;
            }

            result = null;
            return false;
        }
    }
}
