using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate
{
	public class AggregateRepository : IAggregateRepository
	{
		private readonly IEventStore eventStore;
		private readonly IEventBus   eventBus;

		private readonly IDictionary<Guid, AggregateRootBase> aggregateCache;

		public AggregateRepository(IEventBus eventBus, IEventStore eventStore)
		{
			this.eventStore = eventStore;
			this.eventBus = eventBus;

			aggregateCache = new Dictionary<Guid, AggregateRootBase>(); 
		}

		public T GetAggregate<T>(Guid aggregateId) where T : AggregateRootBase
		{
			throw new NotImplementedException();
		}

		public void SaveAggregate<T>(T aggregate) where T : AggregateRootBase
		{
			throw new NotImplementedException();
		}
	}
}
