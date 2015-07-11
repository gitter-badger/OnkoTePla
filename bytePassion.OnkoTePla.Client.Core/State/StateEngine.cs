using System.Collections;


namespace bytePassion.OnkoTePla.Client.Core.State
{
	public interface IStateEngine
	{
		void RegisterState<T>(string stateIdentifier);
		T GetState<T> (string stateIdentifier);
	}

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

		public T GetState<T> (string stateIdentifier)
		{
			return (T)states[stateIdentifier];
		}
	}	
}
