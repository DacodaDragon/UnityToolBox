#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using ToolBox.Editor;

namespace ToolBox.StateMachine.Editor
{
    public static class DrawUtility
    {
        private static readonly Action<CompareFunction> ApplyWireMaterial = (Action<CompareFunction>)Delegate.CreateDelegate(typeof(Action<CompareFunction>), 
            typeof(HandleUtility).GetMethod("ApplyWireMaterial", BindingFlags.Static | BindingFlags.NonPublic,
                Type.DefaultBinder, new Type[] { typeof(CompareFunction) }, new ParameterModifier[0]));

        private static Vector2[] vectors = new Vector2[30];
        private static Vector2[] poly = new Vector2[60];

        public static void DrawCurve(Vector2 start, Vector2 startHandle, Vector2 endHandle, Vector2 end, Color color, float lineWidth)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = MathUtil.Vec2Bezier(start, startHandle, endHandle, end, i / (vectors.Length - 1f));
            }

            MathUtil.PolyLineToTriangleStrip(vectors, ref poly, lineWidth);

            ApplyWireMaterial.Invoke(Handles.zTest);

            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);
            GL.Begin(GL.TRIANGLE_STRIP);
            GL.Color(color);
            
            for (int i = 0; i < poly.Length; i++)
                GL.Vertex(poly[i]);
            
            GL.End();
            GL.PopMatrix();
        }

        public static void DrawNodeWithName(Rect rect, GUIContent label, float shadowSize, float shadowsoftness)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            DrawShadowsOnRect(rect, shadowSize, shadowsoftness);

            EditorGUI.DrawRect(rect, EditorColors.inactiveTabBackground);
            rect = rect.ShrinkFromMiddle(2);
            GUI.BeginGroup(rect);
            rect.position = new Vector2(0, 0);
            EditorGUI.DrawRect(rect, EditorColors.tabBackground);
            DrawText(rect.min + new Vector2(2, 2), label, EditorColors.textColor);
            GUI.EndGroup();
        }

        public static void DrawText(Vector2 pos, GUIContent text, Color color)
        {
            Color c = GUI.color;
            GUI.color = color;
            Handles.Label(pos, text);
            GUI.color = c;
        }

        public static void DrawShadowsOnRect(Rect rect, float size, float softness)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            var transparent = new Color(0, 0, 0, 0);
            var black = new Color(0, 0, 0, softness);

            ApplyWireMaterial.Invoke(Handles.zTest);

            GL.PushMatrix();
            GL.Begin(GL.TRIANGLE_STRIP);
            GL.Color(black);
            GL.Vertex3(rect.xMax, rect.yMin, 0);
            GL.Color(transparent);
            GL.Vertex3(rect.xMax + size, rect.yMin + size, 0);
            GL.Color(black);
            GL.Vertex3(rect.xMax, rect.yMax, 0);
            GL.Color(transparent);
            GL.Vertex3(rect.xMax + size, rect.yMax + size, 0);
            GL.Color(black);
            GL.Vertex3(rect.xMin, rect.yMax, 0);
            GL.Color(transparent);
            GL.Vertex3(rect.xMin + size, rect.yMax + size, 0);
            GL.End();
            GL.PopMatrix();
        }

        public static void DrawShadowsInRect(Rect rect, float size, float softness)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            var transparent = new Color(0, 0, 0, 0);
            var black = new Color(0, 0, 0, softness);

            ApplyWireMaterial.Invoke(Handles.zTest);

            GL.PushMatrix();
            GL.Begin(GL.TRIANGLE_STRIP);
            GL.Color(black);
            GL.Vertex3(rect.x + rect.width, rect.y, 0);
            GL.Color(transparent);
            GL.Vertex3(rect.x + rect.width, rect.y + size, 0);
            GL.Color(black);
            GL.Vertex3(rect.x, rect.y, 0);
            GL.Color(transparent);
            GL.Vertex3(rect.x + size, rect.y + size, 0);
            GL.Color(black);
            GL.Vertex3(rect.x, rect.y + rect.height, 0);
            GL.Color(transparent);
            GL.Vertex3(rect.x + size, rect.y + rect.height, 0);
            GL.End();
            GL.PopMatrix();
        }

        public static void DrawGridInRectWithOffset(Rect rect, float cellSize, Vector2 offset, Color color)
        {
            int verticalLineCount = Mathf.CeilToInt(rect.width / cellSize) + 1;
            int horizontalLineCount = Mathf.CeilToInt(rect.height / cellSize) + 1;

            offset.x %= cellSize;
            offset.y %= cellSize;

            GL.PushMatrix();
            GL.Begin(GL.QUADS);
            GL.Color(color);

            for (int i = 0; i < verticalLineCount; i++)
                GL_VERTICALQUADLINE(rect.position.x + (offset.x + (i * cellSize)), rect.yMin, rect.yMax, 1);

            for (int i = 0; i < horizontalLineCount; i++)
                GL_HORIZONTALQUADLINE(rect.position.y + (offset.y + (i * cellSize)), rect.xMin, rect.xMax, 1);

            GL.End();
            GL.PopMatrix();
        }

        private static void GL_VERTICALQUADLINE(float x, float y1, float y2, float w)
        {
            float wh = (w * 0.5f);

            GL.Vertex3(x - wh, y1, 0);
            GL.Vertex3(x + wh, y1, 0);
            GL.Vertex3(x + wh, y2, 0);
            GL.Vertex3(x - wh, y2, 0);
        }

        private static void GL_HORIZONTALQUADLINE(float y, float x1, float x2, float w)
        {
            float wh = (w * 0.5f);
            GL.Vertex3(x1, y - wh, 0);
            GL.Vertex3(x1, y + wh, 0);
            GL.Vertex3(x2, y + wh, 0);
            GL.Vertex3(x2, y - wh, 0);
        }

        public static void DrawRectBounds(Rect rect, Color color)
        {
            DrawText(rect.min, new GUIContent(rect.min.ToString()), color);
            DrawText(rect.max, new GUIContent(rect.max.ToString()), color);



            DrawLine(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMax), color);
            DrawLine(new Vector2(rect.xMin, rect.yMax), new Vector2(rect.xMax, rect.yMin), color);

            DrawLine(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMin), color);
            DrawLine(new Vector2(rect.xMax, rect.yMin), new Vector2(rect.xMax, rect.yMax), color);
            DrawLine(new Vector2(rect.xMax, rect.yMax), new Vector2(rect.xMin, rect.yMax), color);
            DrawLine(new Vector2(rect.xMin, rect.yMax), new Vector2(rect.xMin, rect.yMin), color);
        }

        public static void DrawLine(Vector2 p1, Vector2 p2, Color color)
        {
            Handles.BeginGUI();
            Handles.color = color;
            Handles.DrawLine(p1, p2);
            Handles.EndGUI();
        }

        public static GenericMenu CreateContextMenu(ContextEntry[] entries)
        {
            var menu = new GenericMenu();
            for (int i = 0; i < entries.Length; i++)
            {
                menu.AddItem(entries[i].name, false, entries[i].function);
            }
            return menu;
        }
    }
}
#endif