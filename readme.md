LazyEnumerable
==============
IEnumerable wrapper library designed to lazily load backing IEnumerable data.
	
	IEnumerable<T> LazyEnumerable<T>(take, (start, take) => { })

Example
-------
	var history = new LazyEnumerable<History>(10, (position, take) =>
	{
		return someApi.GetHistory(position, take);
	});
	
	foreach(var h in history)
	{
		// if(condition)
			break
	
		// Do something
	}

The foreach iterates through the history with every 10th iterations triggering a GetHistory from the api.