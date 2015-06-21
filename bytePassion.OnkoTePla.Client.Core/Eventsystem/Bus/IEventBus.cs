using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.Bus
{

	public interface IEventBus
	{
		void DeregisterEventHandler<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
			where TDomainEvent : DomainEvent;

		void RegisterEventHandler<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
			where TDomainEvent : DomainEvent;

		void Publish<TDomainEvent>(TDomainEvent @event)
			where TDomainEvent : DomainEvent;
	}

}