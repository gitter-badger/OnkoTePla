using System;

namespace bytePassion.Lib.Communication.State
{
	public class GlobalReadOnlyState<T> : IGlobalState<T>
	{
		public event Action<T> StateChanged;

		private readonly T stateValue;

		public GlobalReadOnlyState(T stateValue)
		{
			this.stateValue = stateValue;
		}		

		public T Value
		{
			get { return stateValue; }
			set
			{
				throw new InvalidOperationException("a readonly State cannot be set");
			}
		}
	}
}
