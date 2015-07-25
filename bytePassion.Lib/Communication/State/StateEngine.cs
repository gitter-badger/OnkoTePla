using System.Collections;


namespace bytePassion.Lib.Communication.State
{
	public class StateEngine : IStateEngine
	{

		private readonly IDictionary states;

		public StateEngine()
		{
			states = new Hashtable();
		}

		public void RegisterState<T> (string stateIdentifier)
		{
			states.Add(stateIdentifier, new GlobalState<T>());
		}

		public void RegisterState<T> (string stateIdentifier, T initialValue)
		{
			RegisterState<T>(stateIdentifier);

			var state = GetState<T>(stateIdentifier);
			state.Value = initialValue;
		}

		public GlobalState<T> GetState<T> (string stateIdentifier)
		{
			return (GlobalState<T>)states[stateIdentifier];
		}
	}	
}
