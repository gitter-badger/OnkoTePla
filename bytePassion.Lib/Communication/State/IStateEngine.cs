namespace bytePassion.Lib.Communication.State
{
	public interface IStateEngine
	{
		void RegisterState<T>(string stateIdentifier, T initialValue = default(T));

		IGlobalState<T> GetState<T>(string stateIdentifier);
		
	}
}