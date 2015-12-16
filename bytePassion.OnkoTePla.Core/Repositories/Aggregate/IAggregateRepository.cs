using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Core.Repositories.Aggregate
{
    public interface IAggregateRepository
	{
		AppointmentsOfDayAggregate GetById(AggregateIdentifier aggregateId);
		void Save(AppointmentsOfDayAggregate aggregate);		
	}
}
