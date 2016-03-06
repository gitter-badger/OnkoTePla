using bytePassion.OnkoTePla.Contracts.Domain;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamPersistance
{
	public interface IStreamPersistenceService
    {
        EventStream<AggregateIdentifier> GetEventStream(AggregateIdentifier identifier);
		void FillCacheInitially();
        void PersistStreams();
    }
}