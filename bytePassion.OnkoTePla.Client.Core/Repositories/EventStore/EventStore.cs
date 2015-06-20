using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class EventStore : IEventStore
	{
		private readonly IPersistenceService<IEnumerable<EventStream>> persistenceService;

		private IList<EventStream> eventStreams;
		private readonly IConfigurationReadRepository config;

		public EventStore (IPersistenceService<IEnumerable<EventStream>> persistenceService, IConfigurationReadRepository config)
		{
			eventStreams = new List<EventStream>();
			this.persistenceService = persistenceService;
			this.config = config;
		}		
		
		public EventStream GetEventStream(AggregateIdentifier id)
		{
			var eventStream = eventStreams.FirstOrDefault(evenstream => evenstream.Id == id);

			if (eventStream == null)
			{
				eventStream = new EventStream(new AggregateIdentifier(id.Date, 
																	  id.MedicalPracticeId,
																	  config.GetLatestVersionFor(id.MedicalPracticeId)));
				eventStreams.Add(eventStream);
			}

			return eventStream;
		}

		public void AddEventsToEventStream(AggregateIdentifier id, IEnumerable<DomainEvent> eventStream)
		{						
			GetEventStream(id).AddEvents(eventStream);
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
