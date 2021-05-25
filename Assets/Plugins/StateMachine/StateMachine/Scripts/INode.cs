using System;
using UnityEngine;

namespace ToolBox.StateMachine
{
    public interface INode : IIdentifiable<Guid>
    {
        Rect Position { get; set; }
        string Name { get; }
    }
}
