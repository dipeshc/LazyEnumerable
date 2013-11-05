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
			var position = 0;
            do
            {
                var pagedResults = LazyEnumerableItemLoader (position, Count);

                if(pagedResults == null)
                    break;

                var pagedResultsArray = pagedResults.ToArray();

                if(pagedResultsArray.Length == 0)
                    break;

                position += pagedResultsArray.Length;

                foreach (var item in pagedResultsArray)
                    yield return item;
            }
            while(true);
		}
	}
}