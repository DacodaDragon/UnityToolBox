using System;
using UnityEngine;

namespace ToolBox.StateMachine
{
    public interface IState : INode, IIdentifiable<Guid>
    {
        new Guid Id { get; set; }

        void OnAwake();
        void OnActivate();
        void OnDeactivate();
        void OnUpdate();
        void OnPhysicsUpdate();
    }
}
