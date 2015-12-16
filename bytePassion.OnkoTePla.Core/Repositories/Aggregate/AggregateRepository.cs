using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using System.Linq;


namespace bytePassion.OnkoTePla.Core.Repositories.Aggregate
{
    public class AggregateRepository : IAggregateRepository
	{				
		private readonly IEventStore eventStore;		
		private readonly IEventBus eventBus;
		private readonly IPatientReadRepository patientRepository;
		private readonly IConfigurationReadRepository config;

		public AggregateRepository(IEventBus eventBus, 
								   IEventStore eventStore, 
								   IPatientReadRepository patientRepository, 
								   IConfigurationReadRepository config)
		{ 
			this.eventStore = eventStore;
			this.patientRepository = patientRepository;
			this.config = config;
			this.eventBus = eventBus;			
		}		

		public AppointmentsOfDayAggregate GetById(AggregateIdentifier aggregateId)
		{			
			var eventStream = eventStore.GetEventStreamForADay(aggregateId);
			var aggregate   = new AppointmentsOfDayAggregate(eventStream.Id, patientRepository, config);
			aggregate.LoadFromEventStream(eventStream);

			return aggregate;
		}

		public void Save(AppointmentsOfDayAggregate aggregate)
		{
			var uncommittedChanges = aggregate.GetUncommitedChanges().ToList();

			eventStore.AddEventsToEventStream(aggregate.Id, uncommittedChanges);

			foreach (var @event in uncommittedChanges)			
				eventBus.PublishEvent(Converter.ChangeTo(@event, @event.GetType()));							
		}
	}
}
