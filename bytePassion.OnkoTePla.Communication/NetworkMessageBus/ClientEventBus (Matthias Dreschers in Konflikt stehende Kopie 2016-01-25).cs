using System;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Core.Eventsystem;

namespace bytePassion.OnkoTePla.Communication.NetworkMessageBus
{
	public class ClientEventBus : DisposingObject, IMessageBus<DomainEvent>
	{
		public void RegisterMessageHandler<TMessage> (IMessageHandler<TMessage> messageHandler) where TMessage : DomainEvent
		{
			throw new NotImplementedException();
		}

		public void DeregisterMessageHander<TMessage> (IMessageHandler<TMessage> messageHandler) where TMessage : DomainEvent
		{
			throw new NotImplementedException();
		}

		public void Send<TMessage> (TMessage message) where TMessage : DomainEvent
		{
			throw new NotImplementedException();
		}

		protected override void CleanUp ()
		{

		}
	}
}