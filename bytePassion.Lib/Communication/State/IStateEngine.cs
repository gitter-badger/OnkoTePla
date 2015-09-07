namespace bytePassion.Lib.Communication.State
{
	public interface IStateEngine
	{
		void RegisterState<T>(string stateIdentifier, T initialValue = default(T));
		void RegisterState<T>(string stateIdentifier, IGlobalState<T> newGlobalState);

		IGlobalState<T> GetState<T>(string stateIdentifier);
		
	}
}