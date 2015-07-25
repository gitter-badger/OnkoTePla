namespace bytePassion.Lib.Communication.State
{
	public interface IStateEngine
	{
		void RegisterState<T>(string stateIdentifier);
		void RegisterState<T>(string stateIdentifier, T initialValue);

		GlobalState<T> GetState<T>(string stateIdentifier);
		
	}
}