using System.Collections.Generic;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public interface IStreamManagementService
    {
        List<EventStream<AggregateIdentifier>> LoadInitialEventStreams();
    }
}