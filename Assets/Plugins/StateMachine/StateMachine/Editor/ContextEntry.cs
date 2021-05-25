#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ToolBox.StateMachine.Editor
{
    public struct ContextEntry
    {
        public readonly GUIContent name;
        public readonly GenericMenu.MenuFunction function;

        public ContextEntry(string name, GenericMenu.MenuFunction function)
        {
            this.name = new GUIContent(name);
            this.function = function;
        }
    }
}
#endif