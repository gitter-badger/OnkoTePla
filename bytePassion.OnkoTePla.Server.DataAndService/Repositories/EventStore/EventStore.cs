using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Server.DataAndService.Connection;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamMetaData;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamPersistance;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.EventStore
{
	public class EventStore : IEventStore
	{
		private readonly IStreamPersistenceService streamPersistenceService;
		private readonly IStreamMetaDataService metaDataService;
		private readonly IConnectionService connectionService;


		public EventStore(IStreamPersistenceService streamPersistenceService, 
						  IStreamMetaDataService metaDataService, 
						  IConnectionService connectionService)
		{
			this.streamPersistenceService = streamPersistenceService;
			this.metaDataService = metaDataService;
			this.connectionService = connectionService;
		}


		public EventStream<AggregateIdentifier> GetEventStreamForADay (AggregateIdentifier id)
		{
			return streamPersistenceService.GetEventStream(id);			
		}

		public EventStream<Guid> GetEventStreamForAPatient (Guid patientId)
		{
			var eventStreams = metaDataService.GetDaysForPatient(patientId)
											  .Select(GetEventStreamForADay);

			var eventsForPatient = eventStreams.SelectMany(stream => stream.Events)
											   .Where(@event => @event.PatientId == patientId);

			return new EventStream<Guid>(patientId, eventsForPatient);
		}

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

				var operationSuccessful = eventStream.AddEvent(domainEvent);

				if (!operationSuccessful)
				{
					errorOccured = true;
					break;
				}

				metaDataService.UpdateMetaData(domainEvent);
				connectionService.SendEventNotification(domainEvent);
			}

			return !errorOccured;
		}
		
		public void PersistRepository()
		{
			streamPersistenceService.PersistStreams();
			metaDataService.PersistRepository();			            
		}

		public void LoadRepository()
		{
			streamPersistenceService.FillCacheInitially();
			metaDataService.LoadRepository();	
		}
	}
}
