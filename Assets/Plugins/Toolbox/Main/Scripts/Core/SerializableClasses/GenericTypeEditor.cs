#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using UnityEngine;

namespace ToolBox
{
    [CustomPropertyDrawer(typeof(GenericType))]
    [CanEditMultipleObjects]
    public partial class GenericTypeEditor : PropertyDrawer
    {
        private int _SelectedIndex = 0;
        private string[] types;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            DrawTypeDropDown(position, property);

            EditorGUI.EndProperty();
        }

        private void DrawTypeDropDown(Rect position, SerializedProperty property)
        {
            if (types == null)
            {
                var attribute = property.GetAttribute<InheritsAttribute>();
                types = TypeHelper.GetAllTypesThatInherit(attribute.inheritsFrom)
                    .Select(x => x.FullName.Replace('.', '/')).ToArray();

                var typeName = property.FindPropertyRelative("instanceTypeName").stringValue
                    .Replace('.', '/');

                if (types.TryFindInstanceIndex(typeName, out int index))
                {
                    _SelectedIndex = index;
                }
            }

            EditorGUI.BeginChangeCheck();

            _SelectedIndex = EditorGUI.Popup(position, $"{property.name} type:", _SelectedIndex, types);

            if (EditorGUI.EndChangeCheck())
            {
                property.FindPropertyRelative("instanceTypeName")
                    .stringValue = types[_SelectedIndex].Replace("/", ".");
            }
        }
    }
}
#endif
