LazyEnumerable
==============
IEnumerable wrapper library designed to lazily load backing IEnumerable data.
	
	IEnumerable<T> LazyEnumerable<T>(count, (start, count) => { })

Example
-------
	var history = new LazyEnumerable<History>(10, (start, count) =>
	{
		return someApi.GetHistory(start, count);
	});
	
	foreach(var h in history)
	{
		// if(condition)
			break
	
		// Do something
	}

The foreach iterates through the history with every 10th iterations triggering a GetHistory from the api.
