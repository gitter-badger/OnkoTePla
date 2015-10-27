using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public interface IEventStore : IPersistable
	{						
		void AddEventsToEventStream (AggregateIdentifier id, IEnumerable<DomainEvent> eventStream);

		EventStream<Guid>                GetEventStreamForAPatient(Guid patientId);
		EventStream<AggregateIdentifier> GetEventStreamForADay     (AggregateIdentifier id);
	}
}
