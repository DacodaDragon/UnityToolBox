#if UNITY_EDITOR

using System;

namespace ToolBox.StateMachine.Editor
{
    public abstract class DrawableNodeElement : DrawableElement
    {
        private NodeEditor parent;

        public DrawableNodeElement(NodeEditor area)
        {
            this.parent = area;
        }

        protected void Repaint()
        {
            parent.Repaint();
        }

        protected void NotifyStartDragging(IIdentifiable<Guid> element)
        {
            parent.StartDragging(element);
        }

        protected void NotifyStopDragging()
        {
            parent.StopDragging();
        }

        protected void Delete(Node node)
        {
            parent.Remove(node);
        }

        protected void StartTransition(Node node)
        {
            parent.StartTransition(node);
        }

        public void ProvideParent(NodeEditor area)
        {
            this.parent = area;
        }
    }
}
#endif