using System;


namespace bytePassion.Lib
{
	public static class Generic
	{
		public static bool Equals<T>(T obj1, object obj2, Func<T, T, bool> compareFunc)
		{
			if (obj1 == null && obj2 == null) return true;

			if (obj1 == null) return false;
			if (obj2 == null) return false;

			if (obj1.GetType() != obj2.GetType()) return false;

			var objectAsType = (T)obj2;

			return compareFunc(obj1, objectAsType);
		}
	}
}
