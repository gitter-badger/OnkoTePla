namespace bytePassion.Lib.Messaging
{
	public interface IMessageHandler<in TMessage>
	{
		void Process(TMessage message);
	}
}