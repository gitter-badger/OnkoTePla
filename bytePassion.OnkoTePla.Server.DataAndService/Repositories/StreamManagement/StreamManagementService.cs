using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Domain;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement
{
	public class StreamManagementService : IStreamManagementService
    {
        private readonly IStreamPersistenceService persistenceService;
        private readonly Dictionary<AggregateIdentifier, EventStream<AggregateIdentifier>> cachedStreams = new Dictionary<AggregateIdentifier, EventStream<AggregateIdentifier>>();

        public StreamManagementService(IStreamPersistenceService persistenceService)
        {
            this.persistenceService = persistenceService;
        }

        public EventStream<AggregateIdentifier> GetEventStream(AggregateIdentifier identifier)
        {
            if (cachedStreams.ContainsKey(identifier))
            {
                return cachedStreams[identifier];
            }
            
            var loadedStream = persistenceService.LoadEventStream(identifier);
           cachedStreams.Add(loadedStream.Id, loadedStream);

            return loadedStream;
        }

        public void SaveStreams(IList<EventStream<AggregateIdentifier>> streams)
        {
            persistenceService.SaveStreams(streams);
        }

        public List<EventStream<AggregateIdentifier>> LoadInitialEventStreams()
        {
            return persistenceService.LoadInitialEventStreams();
        }
    }
}