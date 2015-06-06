using System;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate
{
	public interface IAggregateRepository
	{
		T GetAggregate<T>(Guid aggregateId) where T : AggregateRootBase;
		void SaveAggregate<T> (T aggregate) where T : AggregateRootBase;
	}
}
