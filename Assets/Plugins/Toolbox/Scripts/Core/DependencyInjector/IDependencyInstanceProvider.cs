using System;

namespace ToolBox.Injection
{
    abstract class DependencyInstanceProvider : IDependencyInstanceProvider
    {
        protected abstract bool RequestType(Type type, out object result);
        protected virtual void ProvideInjector(Injector injector) { }


        void IDependencyInstanceProvider.ProvideInjector(Injector injector)
            => ProvideInjector(injector);

        bool IDependencyInstanceProvider.RequestType(Type type, out object result)
            => RequestType(type, out result);
    }

    interface IDependencyInstanceProvider
    {
        void ProvideInjector(Injector injector);
        bool RequestType(Type type, out object result);
    }
}
