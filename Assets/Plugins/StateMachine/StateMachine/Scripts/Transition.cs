using System;

namespace ToolBox.StateMachine
{
    [Serializable]
    public class Transition
    {
        public Guid fromState;
        public Guid toState;
    }
}
