using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace bytePassion.Lib.Extensions
{
	public static class PropertyChangedNotifierExtension
	{
		public static void ChangeAndNotify<T> (this PropertyChangedEventHandler handler, object sender,
								   ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return;

			field = value;

			var tmpHandler = handler;
			if (tmpHandler != null)
			{
				tmpHandler(sender, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
