﻿using System.Linq;
using bytePassion.Lib.Messaging;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate
{
	public class AggregateRepository : IAggregateRepository
	{				
		private readonly IEventStore eventStore;		
		private readonly IMessageBus<DomainEvent> eventBus;
		private readonly IPatientReadRepository patientRepository;
		private readonly IConfigurationReadRepository config;

		public AggregateRepository(IMessageBus<DomainEvent> eventBus, IEventStore eventStore, 
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
			var eventStream = eventStore.GetEventStream(aggregateId);
			var aggregate   = new AppointmentsOfDayAggregate(eventStream.Id, patientRepository, config);
			aggregate.LoadFromEventStream(eventStream);

			return aggregate;
		}

		public void Save(AppointmentsOfDayAggregate aggregate)
		{
			var uncommittedChanges = aggregate.GetUncommitedChanges().ToList();

			eventStore.AddEventsToEventStream(aggregate.Id, uncommittedChanges);

			foreach (var @event in uncommittedChanges)			
				eventBus.Send(Converter.ChangeTo(@event, @event.GetType()));							
		}
	}
}
