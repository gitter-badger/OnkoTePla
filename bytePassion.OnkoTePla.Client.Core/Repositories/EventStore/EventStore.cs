using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class EventStore : IEventStore, IPersistablility
	{
		private readonly IPersistenceService<IEnumerable<EventStream>> persistenceService;

		private IList<EventStream> eventStreams;


		public EventStore (IPersistenceService<IEnumerable<EventStream>> persistenceService)
		{
			eventStreams = new List<EventStream>();
			this.persistenceService = persistenceService;
		}

		public EventStreamIdentifier CreateEventStream(Date date, uint configVersion, Guid medicalPracticeId)
		{
			var id = new EventStreamIdentifier(date, configVersion, medicalPracticeId);
			eventStreams.Add(new EventStream(id));
			return id;
		}

		public EventStreamIdentifier? DoesEventStreamExist(Date date, Guid medicalPracticeId)
		{
			var eventStream = GetEventStream(new EventStreamIdentifier(date, 0, medicalPracticeId));

			if (eventStream == null)
				return null;
			else
				return eventStream.Id;
		}

		public EventStream GetEventStream(EventStreamIdentifier id)
		{
			return eventStreams.FirstOrDefault(evenstream => evenstream.Id == id);
		}

		public void AddEventsToEventStream(EventStreamIdentifier id, IEnumerable<DomainEvent> eventStream)
		{			
			var es = GetEventStream(id);
			
			if (es == null)
				throw new ArgumentException("there is no eventStream with that id");
			
			es.AddEvents(eventStream);
		}		

		public void PersistRepository()
		{
			persistenceService.Persist(eventStreams);
		}

		public void LoadRepository()
		{
			eventStreams = persistenceService.Load().ToList();
		}
	}
}
