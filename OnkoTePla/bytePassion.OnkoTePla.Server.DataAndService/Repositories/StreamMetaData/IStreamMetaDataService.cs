using System;
using System.Collections.Generic;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamMetaData
{
	public interface IStreamMetaDataService : IPersistable
	{		
	    void UpdateMetaData(DomainEvent @event);

		IEnumerable<AggregateIdentifier> GetDaysForPatient(Guid patientId);	    
    }
}