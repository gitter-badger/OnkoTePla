namespace bytePassion.Lib.Communication.State
{
	public interface IStateEngine
	{
		void RegisterState<T>(string stateIdentifier, T initialValue = default(T));

		GlobalState<T> GetState<T>(string stateIdentifier);
		
	}
}