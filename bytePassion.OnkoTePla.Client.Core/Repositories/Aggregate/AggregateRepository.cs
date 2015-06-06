using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate
{
	public class AggregateRepository : IAggregateRepository
	{		
		private readonly IConfigurationRepository config;
		private readonly IEventStore eventStore;		
		private readonly IEventBus   eventBus;		

		public AggregateRepository(IEventBus eventBus, IEventStore eventStore, IConfigurationRepository config)
		{ 
			this.eventStore = eventStore;			
			this.eventBus = eventBus;
			this.config = config;
		}		

		public AppointmentsOfDayAggregate GetAppointmentsOfDayAggregate(Date date, Guid medicalPracticeId)
		{

			var id = eventStore.DoesEventStreamExist(date, medicalPracticeId);			

			if (!id.HasValue)			
			{
				var latestMedicalPracticeVersion = config.GetLatestVersionFor(medicalPracticeId);
				id = eventStore.CreateEventStream(date, latestMedicalPracticeVersion, medicalPracticeId);
			}

			var eventStream = eventStore.GetEventStream(id.Value);
			var aggregate   = new AppointmentsOfDayAggregate(id.Value, 0);
			aggregate.LoadFromEventStream(eventStream);

			return aggregate;
		}

		public void SaveAppointsOfADayAggregate(AppointmentsOfDayAggregate aggregate)
		{
			eventStore.AddEventsToEventStream(aggregate.Id, aggregate.GetUncommitedChanges());
		}
	}
}
