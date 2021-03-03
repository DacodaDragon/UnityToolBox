using NUnit.Framework;
using System;
using ToolBox.Collections;

namespace ToolBox.UnitTests
{
	public class ObjecReserving
	{
		class Dummy : IPoolable, IReservable
		{
			public bool IsPooled;
			public bool IsReserved;

			bool IPoolable.IsPooled => IsPooled;
			bool IReservable.IsReserved => IsReserved;

			void IPoolable.Pool()
			{
				IsPooled = true;
			}

			void IReservable.Reserve()
			{
				IsReserved = true;
			}

			void IPoolable.UnPool()
			{
				IsPooled = false;
			}

			void IReservable.UnReserve()
			{
				IsReserved = false;
			}
		}

		ReservableObjectPool<Dummy> CreatePool(int size)
		{
			var dummies = new Dummy[size];
			dummies.SetEach(() => new Dummy());
			ReservableObjectPool<Dummy> instance = null;

			Assert.DoesNotThrow(() => {
				instance = new ReservableObjectPool<Dummy>(dummies);
			}, "Couldn't create ReservableObjectPool", typeof(Exception));

			return instance;
		}

		[Test]
		public void CreatePool()
		{
			CreatePool(10);
		}

		[Test]
		public void ReservePortion()
		{
			var pool = CreatePool(2);
			var section = pool.Reserve(1, "testing");
			Assert.True(section.Count == 1, "Section coun't didn't equal the section size");
			Assert.NotNull(section.GetNext(), "Section did not contain any content");
		}

		[Test]
		public void ReserveMultiplePortions()
		{
			var pool = CreatePool(10);

			var section1 = pool.Reserve(5, "TestOne");
			Assert.True(section1.Count == 5, "Section1 count didn't equal the section size");
			Assert.NotNull(section1.GetNext(), "Section1 element 1 did not return instance");
			Assert.NotNull(section1.GetNext(), "Section1 element 2 did not return instance");
			Assert.NotNull(section1.GetNext(), "Section1 element 3 did not return instance");
			Assert.NotNull(section1.GetNext(), "Section1 element 4 did not return instance");
			Assert.NotNull(section1.GetNext(), "Section1 element 5 did not return instance");
			Assert.Null(section1.GetNext(), "Section1 element 6 did not return instance");

			var section2 = pool.Reserve(5, "TestTwo");
			Assert.True(section2.Count == 5, "Section2 count didn't equal the section size");
			Assert.NotNull(section2.GetNext(), "Section2 element 1 did not return instance");
			Assert.NotNull(section2.GetNext(), "Section2 element 2 did not return instance");
			Assert.NotNull(section2.GetNext(), "Section2 element 3 did not return instance");
			Assert.NotNull(section2.GetNext(), "Section2 element 4 did not return instance");
			Assert.Null(section2.GetNext(), "Section2 element 6 did not return instance");
		}
	}
}
