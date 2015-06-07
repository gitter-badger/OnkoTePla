using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate
{
	public class AggregateRepository : IAggregateRepository
	{				
		private readonly IEventStore eventStore;		
		private readonly IEventBus   eventBus;		

		public AggregateRepository(IEventBus eventBus, IEventStore eventStore)
		{ 
			this.eventStore = eventStore;			
			this.eventBus = eventBus;			
		}		

		public AppointmentsOfDayAggregate GetById(AggregateIdentifier aggregateId)
		{
			
			var eventStream = eventStore.GetEventStream(aggregateId);
			var aggregate   = new AppointmentsOfDayAggregate(aggregateId, 0);
			aggregate.LoadFromEventStream(eventStream);

			return aggregate;
		}

		public void Save(AppointmentsOfDayAggregate aggregate)
		{
			var uncommittedChanges = aggregate.GetUncommitedChanges().ToList();

			eventStore.AddEventsToEventStream(aggregate.Id, uncommittedChanges);

			foreach (var @event in uncommittedChanges)			
				eventBus.Publish(@event);							
		}
	}
}
