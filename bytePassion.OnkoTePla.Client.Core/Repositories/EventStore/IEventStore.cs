using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public interface IEventStore
	{
		IEnumerable<DomainEvent> GetEventStream(Guid aggregateId);
		void SaveEventStream(Guid aggregateId, IEnumerable<DomainEvent> eventStream);		
	}
}
