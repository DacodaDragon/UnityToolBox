using UnityEngine;
using System.Collections.Generic;

namespace ToolBox
{
    public class ObjectPooler<T> where T : class, IPoolable
    {
        private T[] _Pool = default;

        public bool TryActivateNext(out T result)
        {
            if (_Pool.TryFindFirst(x => x.IsAvailable, out T outValue))
            {
                result = outValue;
                result.OnPoolActivate();
                return true;
            }

            result = null;
            return false;
        }

        public ObjectPooler(T[] instances)
        {
#if DEBUG
            if (instances == null)
                new System.ArgumentNullException("A pool is not allowed to be initialized with null");

            if (instances.Length == 0)
                new System.ArgumentException("A pool is not allowed to be ");
#endif
            _Pool = instances;
            _Pool.Each(x => x.OnPoolEnter());
        }
    }

    public abstract class PooledBehaviour : MonoBehaviour, IPoolable
    {
        protected virtual bool IsAvailable { get; set; }
        protected virtual void OnPoolActivate() { }
        protected virtual void OnPoolEnter() { }

        bool IPoolable.IsAvailable => IsAvailable;
        void IPoolable.OnPoolActivate() => OnPoolActivate();
        void IPoolable.OnPoolEnter() => OnPoolEnter();
    }

    public interface IPoolable
    {
        bool IsAvailable { get; }
        void OnPoolEnter();
        void OnPoolActivate();
    }
}