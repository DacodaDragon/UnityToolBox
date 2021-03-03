using System;
using System.Collections.Generic;
using ToolBox.DataHolders;

namespace ToolBox.Collections
{
	public class ReservableObjectPool<T> where T : class, IPoolable, IReservable
	{
		struct Section
		{
			public IntRange range;
			public string purpose;
		}

		T[] _ReservableObjects;
		List<Section> _ReservedSections = new List<Section>(8);

		public ReservableObjectPool(T[] instances)
		{
			_ReservableObjects = instances;
		}

		public ObjectPool<T> Reserve(int count, string forPurpose)
		{
			Section section = FindAvailableSection(count, forPurpose);
			_ReservedSections.Add(section);
			return new ObjectPool<T>(_ReservableObjects, section.range);
		}

		private Section FindAvailableSection(int count, string forPurpose)
		{
			for (int i = _ReservableObjects.Length - count; i >= 0; i--)
			{
				if (_ReservedSections.TryFindFirst(x => x.range.contains(i), out Section value))
				{
					i = value.range.From - (count - 1);
				}
				else
				{
					Section section = new Section();
					section.range = new IntRange(i, i + count);
					section.purpose = forPurpose;
					return section;
				}
			}

			throw new System.Exception("No available section found.");
		}
	}
}
