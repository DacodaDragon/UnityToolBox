#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;

namespace ToolBox.StateMachine.Editor
{
    public abstract class DrawableElement
    {
        protected bool MouseDown(MouseButton mouseButton)
        {
            return Event.current.type == EventType.MouseDown &&
                Event.current.button == (int)mouseButton;
        }

        protected bool MouseUp(MouseButton mouseButton)
        {
            return Event.current.type == EventType.MouseUp &&
                Event.current.button == (int)mouseButton;
        }

        protected bool MouseDrag(MouseButton mouseButton)
        {
            return Event.current.type == EventType.MouseDrag
                && Event.current.button == (int)mouseButton;
        }

        protected void UseEvent()
        {
            Event.current.Use();
        }
    }
}
#endif