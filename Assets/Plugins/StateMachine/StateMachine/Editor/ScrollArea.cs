#if UNITY_EDITOR
using System;
using ToolBox.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToolBox.StateMachine.Editor
{
    public class ScrollArea : DrawableElement
    {
        private Vector2 offset = Vector2.zero;

        private Rect layoutRect;
        private Rect areaRect;
        private Rect viewRect;


        public Vector2 Center => layoutRect.center;

        private bool scrollDragging = false;
        private int controlId = 0;

        public void Draw(Rect rect, Action<Rect> drawWithOffset)
        {
            EditorGUI.DrawRect(rect, new Color(0.12f, 0.12f, 0.12f, 1));
            rect = rect.ShrinkFromMiddle(1);

            HandleOnDrag();

            areaRect = GetContainer();
            StashControlId();

            GUI.BeginGroup(rect);

            rect.position = Vector2.zero;

            if (Event.current.rawType != EventType.Layout)
            {
                layoutRect = rect;
            }

            DrawBackground(layoutRect);
            if (scrollDragging)
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Pan);

            if (drawWithOffset != null)
            {
                drawWithOffset.Invoke(GetViewRect());
            }

            EditorGUIUtility.SetWantsMouseJumping(1);

            DrawUtility.DrawShadowsInRect(rect, 10, 0.3f);

            DrawScrollbars(layoutRect, areaRect);

            HandleOnDragStart();

            GUI.EndGroup();
        }

        private Rect GetViewRect()
        {
            Rect viewRect = new Rect(layoutRect);
            viewRect.position += offset;
            return viewRect;
        }

        public void ReleaseHotControl()
        {
            GUIUtility.hotControl = 0;
        }

        public void SetHotControl()
        {
            GUIUtility.hotControl = controlId;
        }

        private void HandleOnDrag()
        {
            if (!scrollDragging)
                return;


            if (MouseUp(MouseButton.MiddleMouse))
            {
                UnityEngine.Cursor.visible = true;
                scrollDragging = false;
                ReleaseHotControl();
                UseEvent();
                return;
            }

            if (MouseDrag(MouseButton.MiddleMouse))
            {
                offset += Event.current.delta;
                UseEvent();
            }

            if (Event.current.isMouse)
            {
                UseEvent();
            }
        }

        private void StashControlId()
        {
            if (Event.current.rawType == EventType.Layout)
            {
                controlId = GUIUtility.GetControlID(FocusType.Passive);
            }
        }

        private Rect GetContainer()
        {
            Rect AreaRect = new Rect(-1024, -1024, 1024 * 2, 1024 * 2);
            AreaRect.position += offset;
            return AreaRect;
        }

        private void HandleOnDragStart()
        {
            if (MouseDown(MouseButton.MiddleMouse))
            {
                scrollDragging = true;
                SetHotControl();
                UseEvent();
            }
        }

        private void DrawScrollbars(Rect drawRect, Rect area)
        {
            Rect horizonalScrollBarRect = new Rect(
                drawRect.x,
                drawRect.y + (drawRect.height - 13),
                drawRect.width - 13,
                14);
            Rect VerticalScrollBarRect = new Rect(
                drawRect.x + (drawRect.width - 13),
                drawRect.y,
                14,
                drawRect.height - 14);

            Rect moveArea = new Rect(
                (area.x + drawRect.height) - offset.x,
                area.y - offset.y,
                area.width - drawRect.width,
                area.height
                );


            offset.x = GUI.HorizontalScrollbar(horizonalScrollBarRect, offset.x, 0, Mathf.Min(moveArea.xMin, offset.x), Mathf.Max(moveArea.xMax, offset.x));
            offset.y = GUI.VerticalScrollbar(VerticalScrollBarRect, offset.y, 0, Mathf.Min(moveArea.yMin, offset.y), Mathf.Max(moveArea.yMax, offset.y));
        }

        private void DrawBackground(Rect rect)
        {
            EditorGUI.DrawRect(rect, EditorColors.boxColor);
            Color c = EditorColors.textColor;
            c.a = 0.5f;
            DrawUtility.DrawGridInRectWithOffset(rect, 128, offset, c);

        }
    }
}
#endif