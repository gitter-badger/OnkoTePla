using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Domain;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.XMLDataStores
{
	public interface IXmlEventStreamPersistanceService
	{
		void Persist (IEnumerable<EventStream<AggregateIdentifier>> eventStreams, string filename);
		IEnumerable<EventStream<AggregateIdentifier>> Load (string filename);
	}
}