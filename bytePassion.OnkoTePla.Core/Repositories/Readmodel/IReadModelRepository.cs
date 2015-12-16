using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Readmodels;
using System;


namespace bytePassion.OnkoTePla.Core.Repositories.Readmodel
{
    public interface IReadModelRepository
	{
		FixedAppointmentSet GetAppointmentSetOfADay(AggregateIdentifier id, uint eventStreamVersionLimit);
		 
		AppointmentsOfADayReadModel     GetAppointmentsOfADayReadModel    (AggregateIdentifier id);
		AppointmentsOfAPatientReadModel GetAppointmentsOfAPatientReadModel(Guid patientId);
	}
}
