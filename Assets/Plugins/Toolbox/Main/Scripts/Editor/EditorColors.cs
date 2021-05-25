#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace ToolBox.Editor
{
    public static class EditorColors
    {
		private static Color C(string normal, string pro)
        {
			string hex = EditorGUIUtility.isProSkin ? pro : normal;

			if (ColorUtility.TryParseHtmlString(hex, out Color c))
				return c;
			return Color.magenta;
        }

		public static readonly Color boxColor = C("#c8c8c8", "#414141");
		public static readonly Color inactiveTabBackground = C("#a5a5a5", "#282828");
		public static readonly Color tabBackground = C("#c8c8c8", "#383838");
		public static readonly Color fieldBackground = C("#f0f0f0", "#2a2a2a");
		public static readonly Color textColor = C("#565656", "#c2c2c2");
    }
}
#endif
