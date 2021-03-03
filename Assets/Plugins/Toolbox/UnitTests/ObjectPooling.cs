using NUnit.Framework;
using ToolBox.Collections;
using System;

namespace ToolBox.UnitTests
{
	public class ObjectPooling
	{
		private class PoolableDummy : IPoolable
		{
			public bool IsPooled = false;
			bool IPoolable.IsPooled => IsPooled;

			public void Pool()
			{
				IsPooled = true;
			}

			void IPoolable.UnPool()
			{
				IsPooled = false;
			}
		}

		private PoolableDummy[] CreatePoolArray(int size)
		{
			var array = new PoolableDummy[size];
			array.SetEach(() => new PoolableDummy());
			return array;
		}

		private ObjectPool<PoolableDummy> CreateObjectPool(int size)
		{
			return CreateObjectPoolInstance(CreatePoolArray(size));
		}

		private ObjectPool<PoolableDummy> CreateObjectPoolInstance(PoolableDummy[] array)
		{
			ObjectPool<PoolableDummy> dummyPool = null;

			Assert.DoesNotThrow(() =>
			{
				dummyPool = new ObjectPool<PoolableDummy>(array);
			}, "Could not create Object Pool", typeof(Exception));

			return dummyPool;
		}


		[Test]
		public void CreatePool()
		{
			CreateObjectPool(10);
		}

		[Test]
		public void ObjectUnpooled()
		{
			var pool = CreateObjectPool(1);
			var obj = pool.GetNext();
			Assert.False(obj.IsPooled, "Retrieved object still signals it is pooled.");
		}

		[Test]
		public void ObjectPooled()
		{
			var array = CreatePoolArray(1);
			var pool = CreateObjectPoolInstance(array);
			Assert.True(array[0].IsPooled, "Pooled object signals it isn't pooled.");
		}

		[Test]
		public void TakeOne()
		{
			var pool = CreateObjectPool(1);
			var obj = pool.GetNext();
			Assert.NotNull(obj, "Obj recieved was null.");
		}

		[Test]
		public void TakeAll()
		{
			var pool = CreateObjectPool(2);
			Assert.DoesNotThrow(() =>
			{
				var obj1 = pool.GetNext();
				var obj2 = pool.GetNext();

				Assert.NotNull(obj1, "First object was null");
				Assert.NotNull(obj2, "Second object was null");

			}, "Taking all objects caused exception", typeof(Exception));
		}

		[Test]
		public void TakeTooMany()
		{
			var pool = CreateObjectPool(1);
			var obj1 = pool.GetNext();
			var obj2 = pool.GetNext();
			Assert.Null(obj2, "Object wasn't null");
		}

		[Test]
		public void ReturnOne()
		{
			var pool = CreateObjectPool(1);
			var obj1 = pool.GetNext();
			obj1.Pool();
			var obj2 = pool.GetNext();
			Assert.AreEqual(obj1, obj2);
		}
	}
}
