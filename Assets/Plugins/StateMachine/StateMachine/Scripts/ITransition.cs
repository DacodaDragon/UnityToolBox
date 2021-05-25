using System;

namespace ToolBox.StateMachine
{
    interface ITransition
    {
        public Guid FromState { get; set; }
        public Guid ToState { get; set; }

        public void OnEnable();
        public void OnDisable();
        public bool OnValidate();
    }
}
