using System;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement
{
	public interface IStreamMetaDataService
    {
        void PersistMetaData();
	    void UpdateMetaData(DomainEvent @event);

        PracticeMetaData GetMetaDataForPractice(Guid practiceId);
    }
}