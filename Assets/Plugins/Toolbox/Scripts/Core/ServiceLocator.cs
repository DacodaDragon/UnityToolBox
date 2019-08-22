using System;
using System.Collections.Generic;

namespace ToolBox
{
    public static class ServicesNativeMethods
    {
        private static Type[] _ServiceTypes = null;

        private static Type[] GetServices()
        {
            List<Type> types = new List<Type>();

            Action<Type> typeFilter = (y) =>
            {
                if (typeof(IService).IsAssignableFrom(y) && !y.IsInterface) types.Add(y);
            };

            AppDomain.CurrentDomain.GetAssemblies()
                .Each(x => x.GetTypes()
                .Each(typeFilter));

            return types.ToArray();
        }

        public static Type Find<T>()
        {
            if (_ServiceTypes == null)
                _ServiceTypes = GetServices();
            
            return _ServiceTypes.FindMatch<T>();
        }

        public static void Uncache()
        {
            _ServiceTypes = null;
        }
    }

    public interface IService { }
}
