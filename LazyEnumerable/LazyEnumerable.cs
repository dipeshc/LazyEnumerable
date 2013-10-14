using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LazyEnumerable
{
	public delegate IEnumerable<T> LazyEnumerableItemLoader<T>(int position, int take);

	public class LazyEnumerable<T> : IEnumerable<T>
	{
		protected readonly LazyEnumerableItemLoader<T> LazyEnumerableItemLoader;
		protected readonly int Take;

		public LazyEnumerable(int take, LazyEnumerableItemLoader<T> lazyEnumerableItemLoader)
		{
			Take = take;
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
				if (position % Take == 0)
					currentPagedResults = (LazyEnumerableItemLoader (position, Take) ?? new T[] { }).Take(Take).ToArray();
				position += Take;

				paging = currentPagedResults.Count () == Take;

				foreach (var item in currentPagedResults)
					yield return item;
			}
			while(paging);
		}
	}
}