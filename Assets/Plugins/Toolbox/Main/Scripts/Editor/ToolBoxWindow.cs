#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace ToolBox.Editor
{
    public abstract class ToolBoxWindow : EditorWindow
	{
		public bool ButtonPressed(KeyCode key)
		{
			if (Event.current.isKey && Event.current.type == EventType.KeyDown)
			{
				bool isKey = Event.current.keyCode == key;

				if (isKey)
					Event.current.Use();

				return isKey;
			}
			return false;
		}

		public bool OnDoubleClick(int mousebutton)
		{
			if (Event.current.isMouse
			    && Event.current.clickCount == 2
			    && Event.current.type == EventType.MouseDown
			    && Event.current.button == 0)
			{
				Event.current.Use();
				return true;
			}
			return false;
		}
	}
}
#endif
