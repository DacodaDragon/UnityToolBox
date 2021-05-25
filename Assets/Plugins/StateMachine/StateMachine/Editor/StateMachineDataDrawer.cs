#if UNITY_EDITOR
using System.Collections.Generic;
using ToolBox.Editor;
using UnityEditor;
using UnityEngine;
using System;
using ToolBox.Injection;

namespace ToolBox.StateMachine.Editor
{
    [CustomEditor(typeof(StateMachineData))]
    public class StateMachineDataDrawer : UnityEditor.Editor
    {
        private StateMachineData TypedTarget => (StateMachineData)target;

        class TestState : State
        {
            public TestState()
            {
                ((INode)this).Position = new Rect(0, 0, 150, 75);
            }
        }

        NodeEditor _nodeEditor;
        NodeEditor NodeEditor => _nodeEditor ??= new NodeEditor(this, TypedTarget.Nodes,
            TypedTarget.transitions);

        List<Transition> _transitions;

        
        public override bool UseDefaultMargins()
        {
            return true;
        }

        public override bool RequiresConstantRepaint()
        {
            return false;
        }

        public override void OnInspectorGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(500, 500);
            NodeEditor.Draw(rect);
            base.OnInspectorGUI();
        }
    }
}
#endif