namespace ToolBox.StateMachine
{
    public abstract class NamedObject
    {
        private static string _name;
        public string Name => _name ??= GetType().Name;
    }
}
