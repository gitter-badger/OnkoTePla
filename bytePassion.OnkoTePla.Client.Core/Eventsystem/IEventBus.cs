using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
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