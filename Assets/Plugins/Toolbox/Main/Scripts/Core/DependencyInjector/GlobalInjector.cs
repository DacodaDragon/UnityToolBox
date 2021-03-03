using ToolBox.Injection;

namespace ToolBox.Injection
{
    public static class GlobalInjector
	{
		private static readonly Injector _Injector = new Injector();
        public static Injector Injector => _Injector;
    }
}