using System;
using System.Collections.Generic;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.EventStore
{
	public interface IEventStore : IPersistable
	{						
		bool AddEvents (IEnumerable<DomainEvent> newEvents);

		EventStream<Guid>                GetEventStreamForAPatient(Guid patientId);
		EventStream<AggregateIdentifier> GetEventStreamForADay    (AggregateIdentifier id);
	}
}
