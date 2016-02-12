using System;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement
{
	public interface IStreamMetaDataService
    {
        void PersistMetaData();
        //void UpdateMetaDataForPractice(IEnumerable<EventStream<AggregateIdentifier>> streams, AggregateIdentifier identifier);
        //void UpdateMetaDataForPatient(IEnumerable<EventStream<AggregateIdentifier>> streams, Guid patientId);
        void UpdateMetaDataForPractice(DomainEvent @event);
        void UpdateMetaDataForPatient(DomainEvent @event);

        PracticeMetaData GetMetaDataForPractice(Guid practiceId);
    }
}