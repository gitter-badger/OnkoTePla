using System;

namespace bytePassion.Lib.Utils
{
	public interface IWindowBuilder<TWindow>
	{
		TWindow BuildWindow (Action<string> errorCallback);
		void DisposeWindow(TWindow buildedWindow);
	}
}
