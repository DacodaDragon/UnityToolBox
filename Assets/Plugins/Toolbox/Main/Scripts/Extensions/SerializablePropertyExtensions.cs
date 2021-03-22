#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;

namespace ToolBox
{
    public static  class SerializablePropertyExtensions
    {
        public static T GetAttribute<T>(this SerializedProperty property) where T : Attribute
        {
            object target = property.serializedObject.targetObject;
            var type = target.GetType();
            var field = type.GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var attribute = field.GetCustomAttribute<T>();
            return attribute;
        }

        public static object GetRawReference(this SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            var targetObjectClassType = targetObject.GetType();
            var field = targetObjectClassType.GetField(property.propertyPath);
            object value = null;

            if (field != null)
            {
                value = field.GetValue(targetObject);
            }

            return value;
        }

        public static void SetRawReference(this SerializedProperty property, object value)
        {
            var targetObject = property.serializedObject.targetObject;
            var targetObjectClassType = targetObject.GetType();
            var field = targetObjectClassType.GetField(property.propertyPath);

            if (field != null)
            {
                field.SetValue(targetObject, value);
            }
        }
    }
}
#endif