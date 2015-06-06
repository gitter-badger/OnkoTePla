using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class EventStore : IEventStore, IPersistablility
	{
		private readonly IPersistenceService<IDictionary<Guid, IReadOnlyCollection<DomainEvent>>> persistenceService;
		private IDictionary<Guid, IReadOnlyCollection<DomainEvent>> eventStreamCache;


		public EventStore (IPersistenceService<IDictionary<Guid, IReadOnlyCollection<DomainEvent>>> persistenceService)
		{
			eventStreamCache = new Dictionary<Guid, IReadOnlyCollection<DomainEvent>>();
			this.persistenceService = persistenceService;
		}

		public IEnumerable<DomainEvent> GetEventStream(Guid aggregateId)
		{
			return eventStreamCache.ContainsKey(aggregateId) ? eventStreamCache[aggregateId] : null;
		}

		public void SaveEventStream(Guid aggregateId, IEnumerable<DomainEvent> eventStream)
		{
			var eventStreamList = new List<DomainEvent>(eventStream);

			if (eventStreamCache.ContainsKey(aggregateId))
				eventStreamCache[aggregateId] = eventStreamList;
			else
				eventStreamCache.Add(aggregateId, eventStreamList);
		}		

		public void PersistRepository()
		{
			persistenceService.Persist(eventStreamCache);
		}

		public void LoadRepository()
		{
			eventStreamCache = persistenceService.Load();
		}
	}
}
