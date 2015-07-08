using System;


namespace bytePassion.Lib.FrameworkExtensions
{
	public static class EqualsExtensions
	{
		public static bool Equals<T>(this T obj1, object obj2, Func<T, T, bool> compareFunc)
		{						
			if (obj2 == null) return false;

			if (obj1.GetType() != obj2.GetType()) return false;

			var objectAsType = (T)obj2;

			return compareFunc(obj1, objectAsType);
		}
	}
}
