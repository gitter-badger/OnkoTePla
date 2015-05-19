using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;

namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{

	public interface IEventBus
	{
		void Subscribe<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
			where TDomainEvent : DomainEvent;

		void Publish<TDomainEvent>(TDomainEvent @event)
			where TDomainEvent : DomainEvent;
	}

}