#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using ToolBox.Editor;
using ToolBox.Injection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ToolBox.StateMachine.Editor
{
    public class NodeEditor : DrawableElement
    {
        Action<Node> OnNewNodeCreated;

        class BaseTransition : Transition
        {
        }

        enum State
        {
            Idle,
            Dragging,
            TransitionDragging
        }

        List<Node> nodes;
        Dictionary<Guid, Node> nodeDict;
        List<Transition> transitions;
        ScrollArea scrollArea = new ScrollArea();
        UnityEditor.Editor parent;
        Guid draggingId = Guid.Empty;

        Vector2 contextMousePosition;

        private State currentState = State.Idle;

        public readonly GenericMenu areaContext;

        internal NodeEditor(UnityEditor.Editor parent, List<Node> nodes, List<Transition> transitions)
        {
            areaContext = DrawUtility.CreateContextMenu(new[] {
                new ContextEntry("New State", () => OpenStateCreationWindow(scrollArea.Center)),
            });

            this.nodes = nodes;

            foreach (var item in nodes)
                item.ProvideParent(this);

            this.nodeDict = this.nodes.ToArray().ToDict<Node, Guid>();
            this.transitions = transitions;
            this.parent = parent;
        }

        internal void StartTransition(Node node)
        {
            var transition = new BaseTransition();
            transition.fromState = node.Id;
            currentState = State.TransitionDragging;
            Add(transition);
        }

        public void Draw(Rect rect)
        {
            scrollArea.Draw(rect, (x) =>
            {
                if (currentState == State.TransitionDragging && MouseUp(MouseButton.LeftMouse))
                {
                    Vector2 position = Event.current.mousePosition - x.position;
                    Node node = nodes.Find(x => x.Position.Contains(position));

                    if (node != null)
                    {
                        transitions.Where(x => x.toState == Guid.Empty).ToList().ForEach(x => x.toState = node.Id);
                    }
                    else transitions.RemoveAll(x => x.toState == Guid.Empty);
                }

                DrawNodes(x);
                DrawTransitions(x);

                if (MouseDown(MouseButton.RightMouse))
                {
                    contextMousePosition = Event.current.mousePosition - x.position;
                    areaContext.ShowAsContext();
                    UseEvent();
                }
            });


            if (currentState == State.Dragging
                || currentState == State.TransitionDragging)
                Repaint();

            GUILayout.Label(new GUIContent(currentState.ToString()));
        }

        private void DrawNodes(Rect offset)
        {
            if (currentState == State.Dragging && Event.current.isMouse)
            {
                var x = nodes.FirstOrDefault(x => x.Id == draggingId);
                if (x != null)
                    x.Draw(offset.position);
                return;
            }

            if (currentState != State.Dragging && Event.current.type == EventType.MouseDrag)
                return;

            foreach (var node in nodes)
                node.Draw(offset.position);
        }

        private void DrawTransitions(Rect offset)
        {
            foreach (var transition in transitions)
            {
                Vector2 startPoint = nodeDict[transition.fromState].Position.GetPosition(new Vector2(1, 0)) + new Vector2(0, 4);
                Vector2 endPoint;
                if (transition.toState == Guid.Empty)
                {
                    endPoint = Event.current.mousePosition;
                }
                else
                {
                    endPoint = nodeDict[transition.toState].Position.position + new Vector2(0, 4);
                    endPoint += offset.position;
                }

                startPoint += offset.position;

                float handleDistrance = Math.Max(100, Math.Abs((endPoint.x - startPoint.x) * 0.5f));

                DrawUtility.DrawCurve(
                    startPoint,
                    startPoint + new Vector2(handleDistrance, 0),
                    endPoint - new Vector2(handleDistrance, 0),
                    endPoint,
                    EditorColors.inactiveTabBackground, 4);
            }
        }

        public void Repaint()
        {
            parent.Repaint();
        }

        public void StartDragging(IIdentifiable<Guid> element)
        {
            currentState = State.Dragging;
            draggingId = element.Id;
            scrollArea.SetHotControl();
        }

        public void StopDragging()
        {
            currentState = State.Idle;
            scrollArea.ReleaseHotControl();
        }

        public void Remove(Node node)
        {
            nodes.Remove(node);
            nodeDict.Remove(node.Id);
            transitions.RemoveAll(x => x.fromState == node.Id || x.toState == node.Id);
        }

        public void Add(Node node)
        {
            nodes.Add(node);
            nodeDict.Add(node.Id, node);
        }

        internal void Add(Transition transition)
        {
            transitions.Add(transition);
        }

        private void OpenStateCreationWindow(Vector2 position)
        {
            ///var window = TypeSearchWindow.OpenWindow<TypeSearchWindow>(typeof(ToolBox.StateMachine.State), (x) =>
            ///{
            ///    Add(new Node(GenericObjectFactory.CreateObject<INode>(x), this));
            ///});
            ///
            //
            //Vector2 size = new Vector2(300, 300);
            //window.position = new Rect(GUIUtility.GUIToScreenPoint(position) - (size * 0.5f), size);
            var node = new Node(this);
            node.Position = new Rect(contextMousePosition, new Vector2(150, 75));
            Add(node);
        }
    }
}
#endif