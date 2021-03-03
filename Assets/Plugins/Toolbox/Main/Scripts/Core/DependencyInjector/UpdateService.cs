using System;
using UnityEngine;

namespace ToolBox.Services
{
    public class UpdateService : IService
	{
		private class UpdateBehaviour : MonoBehaviour
		{
			public event Action OnUpdate;
			public event Action OnLateUpdate;
			public event Action OnFixedUpdate;

			void Update()
			{
				OnUpdate?.Invoke();
			}

            void LateUpdate()
            {
	            OnLateUpdate?.Invoke();
            }

            private void FixedUpdate()
            {
	            OnFixedUpdate?.Invoke();
            }
        }

		private UpdateBehaviour behavior;

        public event Action OnUpdate
		{
			add { behavior.OnUpdate += value;}
			remove { behavior.OnUpdate -= value; }
		}

		public event Action OnLateUpdate
		{
            add { behavior.OnLateUpdate += value; }
            remove { behavior.OnLateUpdate -= value; }
		}

		public event Action OnFixedUpdate
		{
            add { behavior.OnFixedUpdate += value; }
            remove { behavior.OnLateUpdate -= value; }
		}

        public void GetDependencies(GlobalBehaviourService behaviourService)
        {
	        behavior = behaviourService.GetBehavior<UpdateBehaviour>();
        }
	}
}
