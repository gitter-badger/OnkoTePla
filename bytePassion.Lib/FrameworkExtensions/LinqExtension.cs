using System;
using System.Collections.Generic;

namespace bytePassion.Lib.FrameworkExtensions
{
    public static class LinqExtension
    {
	    public static void Do<TSource>(this IEnumerable<TSource> items, Action<TSource> action)
	    {
		    foreach (var item in items)
		    {
			    action(item);
		    }
	    }
    }
}
