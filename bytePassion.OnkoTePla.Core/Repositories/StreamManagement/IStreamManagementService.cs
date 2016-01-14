using System.Collections.Generic;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public interface IStreamManagementService
    {
        EventStream<AggregateIdentifier> GetEventStream(AggregateIdentifier identifier);
        IList<EventStream<AggregateIdentifier>> GetInitialEventStreams(); 
        void SaveStreams(IList<EventStream<AggregateIdentifier>> stream);
    }
}