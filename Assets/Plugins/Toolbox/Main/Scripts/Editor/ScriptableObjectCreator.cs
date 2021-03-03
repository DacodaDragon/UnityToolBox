#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace ToolBox.Editor
{
	public class ScriptableObjectCreator : ToolBoxWindow
	{
		private Type[] types;
		private Type[] filteredTypes;
		private Rect[] typeRects;
		private string searchString = "";
		private bool FirstTimeFocus = false;
		private int selectedIndex = -1;
		private Vector2 scroll;

		[MenuItem("Assets/Create/Scriptable Object")]
		public static void Init()
		{
			GetWindow<ScriptableObjectCreator>().ShowAuxWindow();
		}

		public void OnFocus()
		{
			titleContent = new GUIContent("Scriptable Object Creator");
			FirstTimeFocus = true;
			types = TypeHelper.GetAllTypesThatInherit<ScriptableObject>().Where(
				x=> !typeof(EditorWindow).IsAssignableFrom(x)
				&& !x.ContainsGenericParameters
				&& !x.FullName.StartsWith("UnityEngine")
				&& !x.FullName.StartsWith("UnityEditor")).ToArray();
			filteredTypes = types;
			typeRects = new Rect[filteredTypes.Length];
		}

		public void OnLostFocus()
		{
			Close();
		}

		public void OnGUI()
		{
			if (ButtonPressed(KeyCode.Escape))
				Close();
			if (ButtonPressed(KeyCode.DownArrow))
				selectedIndex = Math.Min(selectedIndex + 1, filteredTypes.Length);
			if (ButtonPressed(KeyCode.UpArrow))
				selectedIndex = Math.Max(selectedIndex - 1, 0);
			if (OnDoubleClick(0))
				CreateAndClose(filteredTypes[GetCLickedIndex()]);
			if (ButtonPressed(KeyCode.Return) && filteredTypes.Length > 0)
				CreateAndClose(filteredTypes[Mathf.Clamp(selectedIndex, 0, filteredTypes.Length - 1)]);

			DrawSearchBar();
			RenderList();
		}

		private int GetCLickedIndex()
		{
			for (int i = 0; i < typeRects.Length; i++)
			{
				if (typeRects[i].Contains(Event.current.mousePosition + scroll))
					return i -1;
			}
			return -1;
		}

		private void RenderList()
		{
			scroll = GUILayout.BeginScrollView(scroll);
			for (int i = 0; i < filteredTypes.Length; i++)
			{
				if (i == selectedIndex)
				{
					DrawSelectedLabel(filteredTypes[i].FullName);
				}
				else GUILayout.Label(filteredTypes[i].FullName);
				if (Event.current.type == EventType.Repaint)
					typeRects[i] = GUILayoutUtility.GetLastRect();
			}
			GUILayout.EndScrollView();
		}

		private void DrawSelectedLabel(string name)
		{
			Color c = GUI.color;
			GUI.color = Color.yellow;
			GUILayout.Label(name);
			GUI.color = c;
		}

		private Type[] FilterTypes(string searchString)
		{
			return types.Where(x => x.FullName.ToLower().Contains(searchString.ToLower())).ToArray();
		}

		public void DrawSearchBar()
		{
			GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
			GUI.SetNextControlName("SearchBar");
			var newSearchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));

			if (!searchString.Equals(newSearchString))
			{
				filteredTypes = FilterTypes(newSearchString);
				typeRects = new Rect[filteredTypes.Length];
				selectedIndex = -1;
			}

			searchString = newSearchString;

			if (FirstTimeFocus)
			{
				FirstTimeFocus = false;
				GUI.FocusControl("SearchBar");
			}
			GUILayout.EndHorizontal();
		}

		private void CreateAndClose(Type scriptableObject)
		{
			AssetUtil.Create(scriptableObject);
			Close();
		}
	}
}
#endif
