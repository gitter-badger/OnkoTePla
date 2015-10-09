
using System;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Readmodels;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel
{
	public interface IReadModelRepository
	{
		FixedAppointmentSet GetAppointmentSetOfADay(AggregateIdentifier id, uint eventStreamVersionLimit);
		 
		AppointmentsOfADayReadModel     GetAppointmentsOfADayReadModel    (AggregateIdentifier id);
		AppointmentsOfAPatientReadModel GetAppointmentsOfAPatientReadModel(Guid patientId);
	}
}
