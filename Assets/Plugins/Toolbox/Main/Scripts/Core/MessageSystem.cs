using System.Collections.Generic;
using System;

namespace ToolBox
{
    /// <summary>
    /// Static/Singleton implementation of <see cref="MessageSystem"/> used for prototyping
    /// </summary>
    public static class GlobalEvents
    {
        private static readonly MessageSystem _messageSystem = new MessageSystem();
        public static void AddListener<T>(Action<T> callback) where T : Message => _messageSystem.AddListener(callback);
        public static void RemoveListener<T>(Action<T> callback) where T : Message => _messageSystem.RemoveListener(callback);
        public static void SendMessage<T>(T message) where T : Message => _messageSystem.SendMessage(message);
    }

    /// <summary>
    /// Acts as a mediator to send a <see cref="Message"/> to any callback subscribed to that specific type of <see cref="Message"/>
    /// </summary>
    public class MessageSystem
    {
        private Dictionary<Type, List<Delegate>> _Listeners =
            new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Subscribes a callback to a specific type of message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        public void AddListener<T>(Action<T> callback) where T : Message
        {
            if (!_Listeners.ContainsKey(typeof(T)))
            {
                _Listeners.Add(typeof(T), new List<Delegate>());
            }

            _Listeners[typeof(T)].Add(callback);
        }

        /// <summary>
        /// Removes the callback from a specific type of message
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="callback">Callback Instance</param>
        public void RemoveListener<T>(Action<T> callback) where T : Message
        {
            if (_Listeners.TryGetValue(typeof(T), out List<Delegate> methods))
            {
                methods.Remove(callback);
            }
        }

        /// <summary>
        /// Sends a message to all callbacks subscribed to that specific type
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="message">Message Instance</param>
        public void SendMessage<T>(T message) where T : Message
        {
            if (_Listeners.TryGetValue(typeof(T), out List<Delegate> methods))
            {
				for (int i = 0; i < methods.Count; i++)
				{
					((Action<T>)methods[i]).Invoke(message);
				}
            }
        }
    }

    /// <summary>
    /// Base class for a message
    /// used in the  <see cref="MessageSystem"/>
    /// </summary>
    public class Message { }
}
