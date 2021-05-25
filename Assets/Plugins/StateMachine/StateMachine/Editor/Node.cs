#if UNITY_EDITOR
using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEditor;

namespace ToolBox.StateMachine.Editor
{
    [Serializable]
    public class Node : DrawableNodeElement, IIdentifiable<Guid>
    {
        [SerializeField]
        public Guid Id { get; set; } = Guid.NewGuid();
        [SerializeField]
        public Rect Position { get; set; }

        private readonly GenericMenu contextMenu;

        public Node(NodeEditor parentEditor) : base(parentEditor)
        {
            contextMenu = DrawUtility.CreateContextMenu(new[] {
                new ContextEntry("Delete", Delete),
            });
        }

        private void Delete()
        {
            Delete(this);
        }

        private void StartTransition()
        {
            StartTransition(this);
            UseEvent();
        }

        public void Draw(Vector2 scrollOffset)
        {
            HandleNodeDragging(scrollOffset);
            DrawUtility.DrawNodeWithName(Position.OffsetPosition(scrollOffset), new GUIContent("Test"), 4, 0.2f);
        }

        private void HandleNodeDragging(Vector2 offset)
        {
            Rect pos = Position;
            pos.position += offset;


            if (MouseDown(MouseButton.LeftMouse) && Event.current.modifiers == EventModifiers.Control && pos.Contains(Event.current.mousePosition))
            {
                StartTransition();
            }

            if (MouseDrag(MouseButton.RightMouse) && pos.Contains(Event.current.mousePosition))
            {
                StartTransition();
            }

            if (MouseDown(MouseButton.LeftMouse) && pos.Contains(Event.current.mousePosition))
            {
                HandleStartDrag();
            }

            if (MouseDrag(MouseButton.LeftMouse))
            {
                HandleDrag();
            }

            if (MouseUp(MouseButton.LeftMouse))
            {
                HandleEndDrag();
            }

            if (MouseUp(MouseButton.RightMouse) && pos.Contains(Event.current.mousePosition))
            {
                OpenContext();
            }

            if (Event.current.isMouse && pos.Contains(Event.current.mousePosition))
                UseEvent();
        }

        private void OpenContext()
        {
            contextMenu.ShowAsContext();
            UseEvent();
        }

        private void HandleEndDrag()
        {
            NotifyStopDragging();
            UseEvent();
        }

        private void HandleDrag()
        {
            Rect rect = Position;
            rect.position += Event.current.delta;
            Position = rect;
        }

        private void HandleStartDrag()
        {
            NotifyStartDragging(this);
            UseEvent();
        }
    }
}
#endif