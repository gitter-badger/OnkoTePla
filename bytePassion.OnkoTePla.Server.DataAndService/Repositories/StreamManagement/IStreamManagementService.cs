using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Domain;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement
{
	public interface IStreamManagementService
    {
        List<EventStream<AggregateIdentifier>> LoadInitialEventStreams();
    }
}