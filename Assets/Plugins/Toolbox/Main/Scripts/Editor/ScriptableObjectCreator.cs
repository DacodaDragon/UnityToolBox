#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace ToolBox.Editor
{
    public class ScriptableObjectCreator : TypeSearchWindow
    {
        [MenuItem("Assets/Create/Scriptable Object")]
        public static void Init()
        {
            OpenWindow<ScriptableObjectCreator>(typeof(ScriptableObject),
                (x) => {
                    AssetUtil.Create(x);
                    }
                );
        }
    }
}
#endif
