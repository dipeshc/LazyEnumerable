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
			var lazyEnumerable = new LazyEnumerable<int> (5, (start, count) => new [] { 1, 2, 3, 4, 5, 6 });

			var expectedResults = new []
			{
				1, 2, 3, 4, 5, 1, 2, 3, 4, 5
			};

			for (var i = 0; i != 10; ++i)
			{
				var expected = expectedResults [i];
				var actual = lazyEnumerable.ElementAt (i);
				Assert.AreEqual (expected, actual);
			}
		}
	}
}