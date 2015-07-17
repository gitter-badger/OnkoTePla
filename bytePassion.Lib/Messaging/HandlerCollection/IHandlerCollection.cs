using System.Collections.Generic;


namespace bytePassion.Lib.Messaging.HandlerCollection
{
	public interface IHandlerCollection<in TMessageBase>
	{
		void Add <TMessage> (IMessageHandler<TMessage> newMessageHandler) 
			where TMessage : TMessageBase;
		 
		void Remove<TMessage> (IMessageHandler<TMessage> messageHandlerToRemove)
			where TMessage : TMessageBase;

		IEnumerable<IMessageHandler<TMessage>> GetMessageHandler<TMessage> () 
			where TMessage : TMessageBase;
	}
}