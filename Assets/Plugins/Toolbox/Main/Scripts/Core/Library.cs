#if !UNITY_EDITOR
	using System.Collections.Generic;
#endif
using UnityEngine;
using System;

namespace ToolBox
{
    /// <summary>
    /// Contains a set of data editable in the inspector
    /// </summary>
    /// <typeparam name="T1">Element Type</typeparam>
    /// <typeparam name="T2">Type to indentify element with (Either supported by unity inspector or with custom property drawer)</typeparam>
    [Serializable]
    public abstract class Library<T1, T2> : ScriptableObject where T1 : IIdentifiable<T2>
    {
        [SerializeField] public T1[] _elements = default;
#if !UNITY_EDITOR
        private Dictionary<T2, T1> _dictionary;
#endif

        public bool TryResolve(T2 id, out T1 element)
        {
#if UNITY_EDITOR
            return _elements.TryResolve(id, out element);
#else
            if (_dictionary == null)
                _dictionary = _elements.ToDict<T1, T2>();

            return _dictionary.TryGetValue(id, out element);
#endif
        }
    }

    /// <summary>
    /// A simple ID to element mapping for use in <see cref="Library{T1, T2}"/>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    [Serializable]
    public abstract class LibraryElement<T1, T2> : IIdentifiable<T2>
    {
        [SerializeField] private T1 _Element = default;
        [SerializeField] private T2 _Id = default;

        public T1 Element => _Element;
        public T2 Id => _Id;
    }
}
