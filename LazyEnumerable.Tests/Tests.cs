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
			var lazyEnumerable = new LazyEnumerable<int> (10, (position, count) =>
			{
				if (position >= 100)
					return new List<int> ();
				
				var result = Enumerable.Range (position, count).Select (i => i * 2).ToList ();
				
				return result;
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
			var lazyEnumerable = new LazyEnumerable<int> (10, (position, count) =>
			{
				if (position >= 101)
					return new List<int> ();

				var result = Enumerable.Range (position, count).Select (i => i * 2).ToList ();

				return result;
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
			var lazyEnumerable = new LazyEnumerable<int> (10, (position, count) =>
			{
				if (position >= 100)
					return null;

				var result = Enumerable.Range (position, count).Select (i => i * 2).ToList ();

				return result;
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
			var lazyEnumerable = new LazyEnumerable<int> (1000, (position, count) =>
			{
				if (position >= 100)
					return null;

				var result = Enumerable.Range (position, count).Select (i => i * 2).ToList ();

				return result;
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
			var lazyEnumerable = new LazyEnumerable<int> (5, (position, count) => new [] { 1, 2, 3, 4, 5, 6 });

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