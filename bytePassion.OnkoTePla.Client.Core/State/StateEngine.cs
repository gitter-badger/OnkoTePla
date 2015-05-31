using System;

namespace bytePassion.OnkoTePla.Client.Core.State
{
	public class StateEngine
	{

		public void  CreateNewState(Type stateType, string stateName)
		{
			
		}
	}

	public class State <T>
	{
		public event Action<T> StateChanged;

		private T stateValue;

		public State()
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
