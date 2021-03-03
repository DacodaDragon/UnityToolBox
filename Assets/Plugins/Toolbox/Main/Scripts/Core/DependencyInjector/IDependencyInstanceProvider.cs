using System;

namespace ToolBox.Injection
{
    abstract class DependencyInstanceProvider : IDependencyInstanceProvider
    {
        protected Injector currentInjector;

        protected abstract bool RequestType(Type type, out object result);

        private void ProvideInjector(Injector injector)
        {
            currentInjector = injector;

        }


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
