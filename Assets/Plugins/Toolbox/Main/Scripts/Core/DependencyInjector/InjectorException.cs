using System;

namespace ToolBox.Injection
{
    [Serializable]
    public class InjectorException : Exception
    {
        public InjectorException() { }
        public InjectorException(string message) : base(message) { }
        public InjectorException(string message, Exception inner) : base(message, inner) { }
        protected InjectorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
