using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{

	public interface IDomainEventHandlerCollection {

		void Add<TEvent>(IDomainEventHandler<TEvent> newEventHandler)
			where TEvent : DomainEvent;

		IEnumerable<IDomainEventHandler<TEvent>> GetAllDomainEventHandlersFor<TEvent>()
			where TEvent : DomainEvent;
	}

}