using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

namespace ToolBox.Injection
{
    /// <summary>
    /// Injector used for Dependency Injection
    /// </summary>
    public class Injector
    {
	    private Stack<Type> DependencyStack = new Stack<Type>();
        private Dictionary<Type, Type> _TypeToTypeBindings = new Dictionary<Type, Type>();
        private Dictionary<Type, object> _TypeToInstanceBindings = new Dictionary<Type, object>();
        private List<IDependencyInstanceProvider> _Providers = new List<IDependencyInstanceProvider>();

        public void BindTypeTotype<From, To>() where To : From => BindTypeToType(typeof(From), typeof(To));
        public void UnbindTypeToType<From>() => UnbindTypeToType(typeof(From));
        public void BindTypeToInstance<Type, Instance>(Instance instance) where Instance : Type => BindTypeToInstance(typeof(Type), instance);

        private void BindTypeToType(Type type, Type replacement) => _TypeToTypeBindings.Add(type, replacement);
        private void UnbindTypeToType(Type type) => _TypeToTypeBindings.Remove(type);
        private void BindTypeToInstance(Type type, object instance) => _TypeToInstanceBindings.Add(type, instance);
        private void UnbindTypeToInstance(Type type, object instance) => _TypeToInstanceBindings.Add(type, instance);

        public Injector()
        {
	        _Providers.Add(Singleton<ServiceProvider>.Instance);
        }

        public void InjectDependencies(in object target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Type type = target.GetType();
            MethodInfo method = type.GetMethod("GetDependencies");

            if (method == null)
                return;

            object[] parameters = FetchInstances(method.GetParameters());
            method.Invoke(target, parameters);
        }

        public object[] FetchInstancesFromConstructor(in ConstructorInfo info)
        {
            return FetchInstances(info.GetParameters());
        }

        public object[] FetchInstances(in ParameterInfo[] parameters)
        {
            object[] parameterInstances = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameterInstances[i] = FetchInstanceForParameter(parameters[i]);
            }

            return parameterInstances;
        }

        public object FetchInstanceForParameter(in ParameterInfo parameter)
        {
            Type type = parameter.ParameterType;

			// If we bound his type to a certain implemention, convert these
            while (_TypeToTypeBindings.TryGetValue(type, out Type newType))
                type = newType;

            if (DependencyStack.Contains(type))
	            throw new CyclicDependencyResolvingException(DependencyStack, type);

            DependencyStack.Push(type);

            // If we already have an instance of this implementation, return this
            if (_TypeToInstanceBindings.TryGetValue(type, out object value))
            {
                if (value != null)
                {
                    return value;
                }
                else
                {
					// This is an invalid entry, might as well remove it.
                    _TypeToInstanceBindings.Remove(type);
                }
            }

			// Search through the providers to see if any of them can make an instance.
            for (int i = 0; i < _Providers.Count; i++)
            {
	            _Providers[i].ProvideInjector(this);
                if (_Providers[i].RequestType(type, out object result))
                {
                    _TypeToInstanceBindings.Add(type, result);
                    return result;
                }
            }

            UnityEngine.Debug.LogError($"[Injector] No value or definition found, returning NULL for {type.FullName}");
            return null;
        }
    }
}
