using System;
using System.Collections.Generic;
using ToolBox.Debug;
using System.Text;

namespace ToolBox.Injection
{
    [Serializable]
    internal class CyclicDependencyResolvingException : InjectorException
    {
        private Stack<Type> dependencyStack;
        private Type type;

        public CyclicDependencyResolvingException(Stack<Type> dependencyStack, Type type)
        {
            this.dependencyStack = dependencyStack;
            this.type = type;
        }

        public string GetMessage()
        {
	        StringBuilder sb = new StringBuilder();
	        sb.Append($"Resolving dependency {type.FullName} resulted in an infinite loop. \n");

	        sb.Append("\n\nDependency Stack: \n");
	        var list = dependencyStack.ToArray();
	        for (int i = 0; i < list.Length; i++)
	        {
		        var currentType = list[(list.Length - 1) - i];

		        if (i != 0)
			        sb.Append("    depends on: ");

		        sb.Append(currentType.FullName);

		        if (currentType == type)
			        sb.Append("  " + DebugSymbolConstants.RedArrowLeft);
		        sb.Append("\n");
	        }

	        sb.Append("    depends on: " + 
	                  type.FullName + "  " + DebugSymbolConstants.RedArrowLeft + "\n");

	        return sb.ToString();
        }


        public override string Message => GetMessage();

        public override string ToString()
        {
	        return GetMessage();
        }
    }
}