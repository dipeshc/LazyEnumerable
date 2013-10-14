using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LazyEnumerable
{
	public delegate IEnumerable<T> LazyEnumerableItemLoader<T>(int start, int count);

	public class LazyEnumerable<T> : IEnumerable<T>
	{
		protected readonly LazyEnumerableItemLoader<T> LazyEnumerableItemLoader;
		protected readonly int Count;

		public LazyEnumerable(int count, LazyEnumerableItemLoader<T> lazyEnumerableItemLoader)
		{
			Count = count;
			LazyEnumerableItemLoader = lazyEnumerableItemLoader;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			T[] currentPagedResults = null;
			var position = 0;
			var paging = true;

			do
			{
				if (position % Count == 0)
					currentPagedResults = (LazyEnumerableItemLoader (position, Count) ?? new T[] { }).Take (Count).ToArray ();
				position += Count;

				paging = currentPagedResults.Count () == Count;

				foreach (var item in currentPagedResults)
					yield return item;
			}
			while(paging);
		}
	}
}