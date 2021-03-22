using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace ToolBox
{
    public partial class GenericTypeEditor
    {
        public static class EditorScriptUtil
        {
            public static void DrawPropertiesForObject(object target)
            {
                var fields = target.GetType().GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    DrawPropertyForObject(target, fields[i]);
                }
            }

            public static void DrawPropertyForObject(object target, FieldInfo field)
            {
                if (field.FieldType == typeof(int))
                {
                    field.SetValue(target, EditorGUILayout.IntField(new GUIContent(field.Name), (int)field.GetValue(target)));
                }

            }
        }
    }
}
