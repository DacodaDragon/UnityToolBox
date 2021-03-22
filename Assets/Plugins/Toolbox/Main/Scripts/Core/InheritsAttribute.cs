using System;

namespace ToolBox
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class InheritsAttribute : Attribute
    {
        public InheritsAttribute(Type inheritsFrom)
        {
			this.inheritsFrom = inheritsFrom;
        }

        public Type inheritsFrom;
    }
}

