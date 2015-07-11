namespace bytePassion.OnkoTePla.Client.Core.State
{
	public interface IStateEngine
	{
		void RegisterState<T>(string stateIdentifier);
		GlobalState<T> GetState<T>(string stateIdentifier);
	}
}