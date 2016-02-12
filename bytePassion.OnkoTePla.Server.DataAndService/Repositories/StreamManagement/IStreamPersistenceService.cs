using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Domain;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement
{
	public interface IStreamPersistenceService
    {
        EventStream<AggregateIdentifier> LoadEventStream(AggregateIdentifier identifier);
        List<EventStream<AggregateIdentifier>> LoadInitialEventStreams(); 
        void SaveStreams(IList<EventStream<AggregateIdentifier>> stream);
    }
}