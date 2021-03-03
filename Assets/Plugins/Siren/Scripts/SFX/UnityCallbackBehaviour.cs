using UnityEngine;

namespace Siren
{
	public class UnityCallbackBehaviour : MonoBehaviour
	{
		public event System.Action OnUpdate;
		public event System.Action OnLateUpdate;

		private void Update()
		{
			OnUpdate?.Invoke();
		}

		private void LateUpdate()
		{
			OnLateUpdate?.Invoke();
		}
	}
}
