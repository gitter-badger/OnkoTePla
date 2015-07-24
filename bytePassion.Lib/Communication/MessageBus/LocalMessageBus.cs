using bytePassion.Lib.Communication.MessageBus.HandlerCollection;


namespace bytePassion.Lib.Communication.MessageBus
{
	public class LocalMessageBus<TMessageBase> : IMessageBus<TMessageBase>
	{
		private readonly IHandlerCollection<TMessageBase> handlerCollection; 

		public LocalMessageBus(IHandlerCollection<TMessageBase> handlerCollection)
		{
			this.handlerCollection = handlerCollection;
		}

		public void RegisterMessageHandler<TMessage>(IMessageHandler<TMessage> messageHandler) 
			where TMessage : TMessageBase
		{
			handlerCollection.Add(messageHandler);
		}

		public void DeregisterMessageHander<TMessage>(IMessageHandler<TMessage> messageHandler) where TMessage : TMessageBase
		{
			handlerCollection.Remove(messageHandler);
		}

		public void Send<TMessage>(TMessage message) 
			where TMessage : TMessageBase
		{
			var messageHandlerList = handlerCollection.GetMessageHandler<TMessage>();

			if (messageHandlerList == null) return;

			foreach (var messageHandler in messageHandlerList)
			{
				messageHandler.Process(message);
			}
		}
	}
}