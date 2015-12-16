using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate
{
	public interface IAggregateRepository
	{
		AppointmentsOfDayAggregate GetById(AggregateIdentifier aggregateId);
		void Save(AppointmentsOfDayAggregate aggregate);		
	}
}
