﻿using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Server.DataAndService.Connection;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.EventStore
{
	public class EventStore : IEventStore
	{
		private readonly IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> persistenceService;
        private readonly StreamManagementService streamManager;
        private readonly IStreamMetaDataService metaDataService;
        private IList<EventStream<AggregateIdentifier>> eventStreams;
		private readonly IConfigurationReadRepository config;
		private readonly IConnectionService connectionService;

		public EventStore (IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> persistenceService 
						  , StreamManagementService streamManager,
                          IStreamMetaDataService metaDataService, 
						  IConfigurationReadRepository config, 
						  IConnectionService connectionService)
		{
			eventStreams = new List<EventStream<AggregateIdentifier>>();
			this.persistenceService = persistenceService;
		    this.metaDataService = metaDataService;
            this.streamManager = streamManager;
            this.config = config;
			this.connectionService = connectionService;
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

//        public EventStream<AggregateIdentifier> GetEventStreamForADay(AggregateIdentifier id)
//        {
//            var eventStream = eventStreams.FirstOrDefault(evenstream => evenstream.Id == id);
//
//            if (eventStream == null)
//            {
//                eventStream = streamManager.GetEventStream(id);
//            	eventStreams.Add(eventStream);
//            }
//
//            return eventStream;
//        }
       
		public bool AddEvents(IEnumerable<DomainEvent> newEvents)
		{
			var errorOccured = false;

			foreach (var domainEvent in newEvents)
			{
				var eventStream = GetEventStreamForADay(domainEvent.AggregateId);

				var evenstreamAggregateVersion = eventStream.Events.Any() 
													? eventStream.Events.Last().AggregateVersion + 1
													: 0;

				if (evenstreamAggregateVersion != domainEvent.AggregateVersion)
				{
					errorOccured = true;
					break;
				}

                // trigger MetaDataUpdate
                metaDataService.UpdateMetaData(domainEvent);

				eventStream.AddEvent(domainEvent);
				connectionService.SendEventNotification(domainEvent);
			}

			return !errorOccured;
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
            //streamManager.SaveStreams(eventStreams);
            metaDataService.PersistMetaData();
		}

		public void LoadRepository()
		{
			eventStreams = persistenceService.Load().ToList();
		    metaDataService.Initialize();
		    //eventStreams = streamManager.LoadInitialEventStreams();
		}
	}
}
