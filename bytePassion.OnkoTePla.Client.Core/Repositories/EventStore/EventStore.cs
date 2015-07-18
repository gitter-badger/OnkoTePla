﻿using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class EventStore : IEventStore
	{
		private readonly IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> persistenceService;

		private IList<EventStream<AggregateIdentifier>> eventStreams;
		private readonly IConfigurationReadRepository config;

		public EventStore (IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> persistenceService, 
						   IConfigurationReadRepository config)
		{
			eventStreams = new List<EventStream<AggregateIdentifier>>();
			this.persistenceService = persistenceService;
			this.config = config;
		}

		public EventStream<AggregateIdentifier> GetEventStream (AggregateIdentifier id)
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
			GetEventStream(id).AddEvents(eventStream);
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
		}

		public void LoadRepository()
		{
			eventStreams = persistenceService.Load().ToList();
		}
	}
}
