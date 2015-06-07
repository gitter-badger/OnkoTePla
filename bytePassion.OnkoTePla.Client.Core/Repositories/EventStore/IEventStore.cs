using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public interface IEventStore
	{				
		EventStream GetEventStream(AggregateIdentifier id);
		void AddEventsToEventStream (AggregateIdentifier id, IEnumerable<DomainEvent> eventStream);		
	}
}
