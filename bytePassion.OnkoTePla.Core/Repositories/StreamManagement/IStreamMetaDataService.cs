using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
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