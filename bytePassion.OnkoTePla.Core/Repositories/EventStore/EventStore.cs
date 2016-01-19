using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Core.Repositories.StreamManagement;


namespace bytePassion.OnkoTePla.Core.Repositories.EventStore
{
    public class EventStore : IEventStore
	{
		private readonly IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> persistenceService;
        private readonly StreamPersistenceService streamManager;

        private IList<EventStream<AggregateIdentifier>> eventStreams;
		private readonly IConfigurationReadRepository config;

		public EventStore (IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> persistenceService, StreamPersistenceService streamManager, IConfigurationReadRepository config)
		{
			eventStreams = new List<EventStream<AggregateIdentifier>>();
			this.persistenceService = persistenceService;
		    this.streamManager = streamManager;
		    this.config = config;
		}

		public EventStream<AggregateIdentifier> GetEventStreamForADay (AggregateIdentifier id)
		{
			var eventStream = eventStreams.FirstOrDefault(evenstream => evenstream.Id == id);

			if (eventStream == null)
			{
				eventStream = new EventStream<AggregateIdentifier>(new AggregateIdentifier(id.Date, 
																	                       id.MedicalPracticeId,
																	                       config.GetLatestVersionFor(id.MedicalPracticeId)));
				eventStreams.Add(eventStream);
			}

			return eventStream;
		}

		public void AddEventsToEventStream(AggregateIdentifier id, IEnumerable<DomainEvent> eventStream)
		{						
			GetEventStreamForADay(id).AddEvents(eventStream);
		}

		public EventStream<Guid> GetEventStreamForAPatient(Guid patientId)
		{
			var eventsForPatient = eventStreams.SelectMany(stream => stream.Events)
				                               .Where(@event => @event.PatientId == patientId);

			return new EventStream<Guid>(patientId, eventsForPatient);
		}

		public void PersistRepository()
		{
			persistenceService.Persist(eventStreams);
            streamManager.SaveStreams(eventStreams);
		}

		public void LoadRepository()
		{
			eventStreams = persistenceService.Load().ToList();
		    //eventStreams = streamManager.LoadInitialEventStreams();
		}
	}
}
