using System;


namespace bytePassion.Lib.Communication.State
{
	public interface IGlobalState<T>
	{
		event Action<T> StateChanged;

		T Value { get; set; }
	}
}