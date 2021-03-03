using UnityEngine;

namespace ToolBox.Debug
{
    internal static class Log
    {
	    private static ILogger logger = new NamedUnityLogger("ToolBox");
    }
}