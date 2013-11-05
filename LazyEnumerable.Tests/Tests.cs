using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LazyEnumerable.Tests
{
	public class Tests
	{
		[Test]
		public void BasicIntRangeTest()
		{
			var lazyEnumerable = new LazyEnumerable<int> (10, (start, count) =>
			{
				if (start >= 100)
					return new List<int> ();
				
				return Enumerable.Range (start, count).Select (i => i * 2).ToList ();
			});

			var expectedPositionCounter = 0;
			foreach (var item in lazyEnumerable)
			{
				var expected = expectedPositionCounter++ * 2;
				Assert.AreEqual (expected, item);
			}
		}

		[Test]
		public void FinishReturningInNonMod0PositionTest()
		{
			var lazyEnumerable = new LazyEnumerable<int> (10, (start, count) =>
			{
				if (start >= 101)
					return new List<int> ();

				return Enumerable.Range (start, count).Select (i => i * 2).ToList ();
			});

			var expectedPositionCounter = 0;
			foreach (var item in lazyEnumerable)
			{
				var expected = expectedPositionCounter++ * 2;
				Assert.AreEqual (expected, item);
			}
		}

		[Test]
		public void FinishReturningWithNullResponseTest()
		{
			var lazyEnumerable = new LazyEnumerable<int> (10, (start, count) =>
			{
				if (start >= 100)
					return null;

				return Enumerable.Range (start, count).Select (i => i * 2).ToList ();
			});

			var expectedPositionCounter = 0;
			foreach (var item in lazyEnumerable)
			{
				var expected = expectedPositionCounter++ * 2;
				Assert.AreEqual (expected, item);
			}
		}

		[Test]
		public void ReturnNonFullCountInFirstRequestResponseTest()
		{
			var lazyEnumerable = new LazyEnumerable<int> (1000, (start, count) =>
			{
				if (start >= 100)
					return null;

				return Enumerable.Range (start, count).Select (i => i * 2).ToList ();
			});

			var expectedPositionCounter = 0;
			foreach (var item in lazyEnumerable)
			{
				var expected = expectedPositionCounter++ * 2;
				Assert.AreEqual (expected, item);
			}
		}

		[Test]
		public void ReturnMoreThanTake()
		{
            var calls = 0;
			var lazyEnumerable = new LazyEnumerable<int> (5, (start, count) =>
            {
                if(++calls == 2)
                    return null;

                return new [] { 1, 2, 3, 4, 5, 6, 7, 8 };
            });

            var i = 0;
            foreach (var item in lazyEnumerable)
            {
                if (++i == 7)
                    break;
            }

            Assert.AreEqual(1, calls);
		}
	}
}