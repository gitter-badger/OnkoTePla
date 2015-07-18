﻿namespace bytePassion.Lib.Messaging
{
	public interface IMessageBus<in TMessageBase>
	{
		void RegisterMessageHandler<TMessage>(IMessageHandler<TMessage> messageHandler) 
			where TMessage : TMessageBase;

		void DeregisterMessageHander<TMessage>(IMessageHandler<TMessage> messageHandler)
			where TMessage : TMessageBase;

		void Send<TMessage>(TMessage message) 
			where TMessage : TMessageBase;
	}
}