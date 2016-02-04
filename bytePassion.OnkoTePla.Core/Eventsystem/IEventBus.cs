using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.OnkoTePla.Core.Eventsystem
{
	public interface IEventBus
	{
		void RegisterEventHandler<TDomainEvent>(IMessageHandler<TDomainEvent> eventHandler) 
			where TDomainEvent : DomainEvent;
		
		void DeregisterEventHander<TDomainEvent>(IMessageHandler<TDomainEvent> eventHandler)
			where TDomainEvent : DomainEvent;
		
		void PublishEvent<TDomainEvent>(TDomainEvent @event) 
			where TDomainEvent : DomainEvent;
	}
}
