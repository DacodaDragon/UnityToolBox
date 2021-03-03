using UnityEngine;
using System.Collections.Generic;
using System;
using ToolBox.DataHolders;

namespace ToolBox.Collections
{
	public class ObjectPool<T> where T : class, IPoolable
	{
		public readonly int Count;

		private readonly int _FromIndex;
		private readonly int _ToIndex;
		private readonly T[] _PooledObjects;

		private int _Index;

		public ObjectPool(T[] instances)
		{
			_PooledObjects = instances;
			_FromIndex = 0;
			_ToIndex = instances.Length;
			Count = _ToIndex - _FromIndex;
			_Index = _FromIndex;
			PoolAll();
		}

		public ObjectPool(ArraySegment<T> segment)
		{
			_PooledObjects = segment.Array;
			_FromIndex = segment.Offset;
			_ToIndex = segment.Offset + segment.Count;
			Count = _ToIndex - _FromIndex;
			_Index = _FromIndex;
			PoolAll();
		}

		public ObjectPool(T[] array, IntRange range)
		{
			_PooledObjects = array;
			_FromIndex = range.From;
			_ToIndex = range.To;
			Count = _ToIndex - _FromIndex;
			_Index = _FromIndex;
			PoolAll();
		}

		private void PoolAll()
		{
			for (int i = _FromIndex; i < _ToIndex; i++)
			{
				_PooledObjects[i].Pool();
			}
		}

		public T GetNext()
		{
			for (int i = _Index; i < _ToIndex; i++)
			{
				if (_PooledObjects[i].IsPooled)
				{
					_Index = i;
					_PooledObjects[i].UnPool();
					return _PooledObjects[i];
				}
			}

			for (int i = _FromIndex; i <= _Index; i++)
			{
				if (_PooledObjects[i].IsPooled)
				{
					_Index = i;
					_PooledObjects[i].UnPool();
					return _PooledObjects[i];
				}
			}

			return null;
		}
	}
}
