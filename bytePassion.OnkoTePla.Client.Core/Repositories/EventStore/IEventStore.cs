using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public interface IEventStore : IPersistable
	{						
		void AddEventsToEventStream (AggregateIdentifier id, IEnumerable<DomainEvent> eventStream);

		EventStream<Guid>                GetEventStreamForAPatient(Guid patientId);
		EventStream<AggregateIdentifier> GetEventStreamForADay     (AggregateIdentifier id);
	}
}
