﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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


	    public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> items)
	    {
		    return new ObservableCollection<TSource>(items);
	    }


	    public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> items, TSource itemToAppend)
	    {
		    return items.Concat(new List<TSource> { itemToAppend });
	    } 
    }
}
