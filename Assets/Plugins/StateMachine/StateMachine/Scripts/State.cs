using System;
using UnityEngine;

namespace ToolBox.StateMachine
{
    [Serializable]
    public abstract class State : NamedObject, IState
    {
        [SerializeField]
        private Guid Id { get; set; } = Guid.NewGuid();
        [SerializeField]
        private Rect EditorLocation { get; set; }

        public virtual void OnAwake() { }
        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }
        public virtual void OnPhysicsUpdate() { }
        public virtual void OnUpdate() { }

        Guid IIdentifiable<Guid>.Id => Id;
        Guid IState.Id { get => Id; set => Id = value; }
        Rect INode.Position { get => EditorLocation; set => EditorLocation = value; }

        void IState.OnAwake() => OnAwake();
        void IState.OnActivate() => OnActivate();
        void IState.OnDeactivate() => OnDeactivate();
        void IState.OnPhysicsUpdate() => OnPhysicsUpdate();
        void IState.OnUpdate() => OnUpdate();
    }
}
