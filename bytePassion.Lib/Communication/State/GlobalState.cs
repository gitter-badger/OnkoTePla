using System;


namespace bytePassion.Lib.Communication.State
{
	public class GlobalState<T>
	{
		public event Action<T> StateChanged;

		private T stateValue;


		public GlobalState()
		{
			stateValue = default(T);			
		}

		public T Value
		{
			get { return stateValue; }
			set
			{
				if (value.Equals(stateValue)) return;

				stateValue = value;					
				var handlers = StateChanged;

				if (handlers != null)				
					handlers(stateValue);				
			}
		}		
	}
}