using System.Collections;


namespace bytePassion.OnkoTePla.Client.Core.State
{
	public class StateEngine : IStateEngine
	{

		private readonly IDictionary states;

		public StateEngine()
		{
			states = new Hashtable();
		}

		public void RegisterState<T>(string stateIdentifier)
		{
			states.Add(stateIdentifier, new GlobalState<T>());
		}

		public GlobalState<T> GetState<T> (string stateIdentifier)
		{
			return (GlobalState<T>)states[stateIdentifier];
		}
	}	
}
