namespace bytePassion.Lib.Communication.MessageBus
{
	public interface IRequestHandler<in TMessage, out TResult>
	{
		TResult Process(TMessage message);
	}
}
