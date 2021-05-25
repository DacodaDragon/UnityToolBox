using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ToolBox.StateMachine.Editor;
using UnityEngine;

namespace ToolBox.StateMachine
{
    [Serializable]
    internal class StateMachineData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private string stateData;
        [SerializeField] private string transitionData;
        [SerializeField] public Guid startState;

        [NonSerialized] public List<Node> Nodes = new List<Node>();
        [NonSerialized] public List<Transition> transitions = new List<Transition>();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            stateData = Json.From(Nodes);
            transitionData = Json.From(transitions);
        }
        
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Nodes = Json.To<List<Node>>(stateData);
            transitions = Json.To<List<Transition>>(transitionData);
        }
    }
}
