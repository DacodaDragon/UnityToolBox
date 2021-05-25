#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;


namespace ToolBox.Editor
{
    public class TypeSearchWindow : ToolBoxWindow
    {
        public event Action<Type> OnTypeSelected;

        private Type searchType;
        private Type[] types;
        private Type[] filteredTypes;
        private Rect[] typeRects;
        private string searchString = "";
        private bool FirstTimeFocus = false;
        private int selectedIndex = -1;
        private Vector2 scroll;

        public void OnFocus()
        {
            FirstTimeFocus = true;
        }

        public void Awake()
        {
           
        }

        public void OnLostFocus()
        {
            Close();
        }

        public TypeSearchWindow Open(Type searchType)
        {
            this.searchType = searchType;
            titleContent = new GUIContent($"Type Search");
            types = TypeHelper.GetAllTypesThatInherit(searchType).Where(
                x => !typeof(EditorWindow).IsAssignableFrom(x)
                && !x.ContainsGenericParameters
                && !x.IsAbstract
                && !x.FullName.StartsWith("UnityEngine")
                && !x.FullName.StartsWith("UnityEditor")).ToArray();
            filteredTypes = types;
            typeRects = new Rect[filteredTypes.Length];
            ShowUtility();
            return this;
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
                SelectType(GetCLickedIndex());
            if (ButtonPressed(KeyCode.Return) && filteredTypes.Length > 0)
                SelectType(filteredTypes[Mathf.Clamp(selectedIndex, 0, filteredTypes.Length - 1)]);

            DrawSearchBar();
            RenderList();
        }

        private void SelectType(int typeIndex)
        {
            if (typeIndex != -1)
                SelectType(filteredTypes[typeIndex]);
        }

        private int GetCLickedIndex()
        {
            for (int i = 0; i < typeRects.Length; i++)
            {
                Vector2 mousePosition = Event.current.mousePosition + scroll + new Vector2(0, -17);
                Rect buttonRect = typeRects[i];

                if (buttonRect.Contains(mousePosition))
                    return i;
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
                    DrawSelectedLabel(filteredTypes[i].Name);
                }
                else GUILayout.Label(filteredTypes[i].Name);
                
                if (Event.current.rawType != EventType.Layout)
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

            searchString = newSearchString;

            if (FirstTimeFocus)
            {
                FirstTimeFocus = false;
                GUI.FocusControl("SearchBar");
            }
            GUILayout.EndHorizontal();

            if (!searchString.Equals(newSearchString))
            {
                filteredTypes = FilterTypes(newSearchString);
                typeRects = new Rect[filteredTypes.Length];
                selectedIndex = -1;
            }
        }

        private void SelectType(Type scriptableObject)
        {
            OnTypeSelected?.Invoke(scriptableObject);
            Close();
        }

        public static TypeSearchWindow OpenWindow(Type type, Action<Type> OntypeSelected)
        {
            return OpenWindow<TypeSearchWindow>(type, OntypeSelected);
        }

        public static TypeSearchWindow OpenWindow<T>(Type type, Action<Type> OntypeSelected) where T : TypeSearchWindow
        {
            if (OntypeSelected == null) throw new ArgumentNullException(nameof(OntypeSelected));

            T window = (T)CreateInstance<T>().Open(type);
            window.OnTypeSelected += OntypeSelected;
            return window;
        }
    }
}
#endif
