using UnityEngine;

namespace ToolBox.Services
{
    public class GlobalBehaviourService : IService
	{
		public GameObject _GameObject;

        public GlobalBehaviourService()
        {
            _GameObject = new GameObject("Service Behaviors");
            Object.DontDestroyOnLoad(_GameObject);
            _GameObject.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            _GameObject.isStatic = true;
        }

		public T GetBehavior<T>() where T : MonoBehaviour, new()
		{
			if (_GameObject.TryGetComponent(out T component))
			{
				return component;
			}

			return _GameObject.AddComponent<T>();
		}
	}
}
