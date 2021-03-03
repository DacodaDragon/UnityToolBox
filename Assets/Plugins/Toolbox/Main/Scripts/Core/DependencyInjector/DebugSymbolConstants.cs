namespace ToolBox.Debug
{
    internal static class DebugSymbolConstants
	{
		public const string RedArrowLeft = "<color=#FF0000><b>←</b></color>";
		public const string NametagOpen = "[";
		public const string NameTagClose = "]";

        public static string FormatNametag(string name) => NametagOpen + name + NameTagClose;
    }
}