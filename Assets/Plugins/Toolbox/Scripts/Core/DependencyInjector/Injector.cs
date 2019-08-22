using System.Collections.Generic;
using System.Reflection;
using System;

namespace ToolBox.Injection
{
    /// <summary>
    /// Injector used for Dependency Injection
    /// </summary>
    class Injector
    {
        private Dictionary<Type, Type> _TypeToTypeBindings = new Dictionary<Type, Type>();
        private Dictionary<Type, object> _TypeToInstanceBindings = new Dictionary<Type, object>();
        private List<IDependencyInstanceProvider> _Providers = new List<IDependencyInstanceProvider>();

        public void BindTypeToType(Type type, Type replacement) => _TypeToTypeBindings.Add(type, replacement);
        public void UnbindTypeToType(Type type) => _TypeToTypeBindings.Remove(type);
        public void BindTypeToInstance(Type type, object instance) => _TypeToInstanceBindings.Add(type, instance);
        public void UnbindTypeToInstance(Type type, object instance) => _TypeToInstanceBindings.Add(type, instance);

        public void InjectDependencies(object target, string methodName = "GetDependencies")
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Type type = target.GetType();
            MethodInfo method = type.GetMethod(methodName);

            if (method == null)
                return;

            object[] parameters = FetchInstances(method.GetParameters());
            method.Invoke(target, parameters);
        }

        public object[] FetchInstancesFromConstructor(ConstructorInfo info)
        {
            return FetchInstances(info.GetParameters());
        }

        public object[] FetchInstances(ParameterInfo[] parameters)
        {
            object[] parameterInstances = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameterInstances[i] = FetchType(parameters[i].ParameterType);
            }

            return parameterInstances;
        }

        public object FetchType(Type type)
        {
            if (_TypeToTypeBindings.TryGetValue(type, out Type newType))
                type = newType;

            if (_TypeToInstanceBindings.TryGetValue(type, out object value))
            {
                if (value != null)
                {
                    return value;
                }
                else
                {
                    _TypeToInstanceBindings.Remove(type);
                }
            }

            for (int i = 0; i < _Providers.Count; i++)
            {
                if (_Providers[i].RequestType(type, out object result))
                {
                    _TypeToInstanceBindings.Add(type, result);
                    return result;
                }
            }

            Console.WriteLine($"[DI] No instance found of type: {type.Name}");

            return null;
        }
    }
}
