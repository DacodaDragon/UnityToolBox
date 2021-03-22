using System.Linq;
using UnityEditor;

namespace ToolBox
{
    public static class SerializedObjectHelper
    {
        public static SerializedProperty[] GetAllProperties(this SerializedObject obj)
        {
            return TypeHelper.GetFieldNames(obj).Select(x => obj.FindProperty(x)).ToArray();
        }
    }
}

