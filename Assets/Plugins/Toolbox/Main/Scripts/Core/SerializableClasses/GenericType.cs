using ToolBox.Injection;
using UnityEngine;

namespace ToolBox
{
    [System.Serializable]
    public class GenericType : ISerializationCallbackReceiver
    {
        [SerializeField]
        private string instanceData = "";

        [SerializeField]
        private string instanceTypeName;

        private object instance;

        public T GetType<T>() where T : class
        {
            return (T)instance;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(instanceTypeName))
                return;
            
            instance = GenericObjectFactory.CreateObject(instanceTypeName,
                       GlobalInjector.Injector,
                       instanceData);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (instance != null)
            {
                instanceData = JsonUtility.ToJson(instance);
            }
        }
    }
}
