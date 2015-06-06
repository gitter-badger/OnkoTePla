using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public interface IEventStore
	{
		EventStreamIdentifier CreateEventStream(Date date, uint configVersion, Guid medicalPracticeId);
		EventStreamIdentifier? DoesEventStreamExist(Date date, Guid medicalPracticeId);
		EventStream GetEventStream(EventStreamIdentifier id);
		void AddEventsToEventStream (EventStreamIdentifier id, IEnumerable<DomainEvent> eventStream);		
	}
}
