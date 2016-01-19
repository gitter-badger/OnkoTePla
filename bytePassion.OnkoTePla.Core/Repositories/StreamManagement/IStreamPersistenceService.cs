using System.Collections.Generic;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public interface IStreamPersistenceService
    {
        EventStream<AggregateIdentifier> LoadEventStream(AggregateIdentifier identifier);
        List<EventStream<AggregateIdentifier>> LoadInitialEventStreams(); 
        void SaveStreams(IList<EventStream<AggregateIdentifier>> stream);
    }
}