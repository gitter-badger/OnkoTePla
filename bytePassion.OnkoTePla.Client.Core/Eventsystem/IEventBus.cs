using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{

	public interface IEventBus
	{
		void RegisterEventHandler<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
			where TDomainEvent : DomainEvent;

		void Publish<TDomainEvent>(TDomainEvent @event)
			where TDomainEvent : DomainEvent;
	}

}