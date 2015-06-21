using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public interface IEventStore : IPersistable
	{				
		EventStream GetEventStream(AggregateIdentifier id);
		void AddEventsToEventStream (AggregateIdentifier id, IEnumerable<DomainEvent> eventStream);		
	}
}
