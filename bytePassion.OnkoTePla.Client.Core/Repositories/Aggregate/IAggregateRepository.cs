using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate
{
	public interface IAggregateRepository
	{

		AppointmentsOfDayAggregate GetAppointmentsOfDayAggregate(Date date, Guid medicalPracticeId);
		void SaveAppointsOfADayAggregate(AppointmentsOfDayAggregate aggregate);

		
	}
}
