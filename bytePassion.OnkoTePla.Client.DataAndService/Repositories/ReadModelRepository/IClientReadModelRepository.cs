using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels;
using bytePassion.OnkoTePla.Contracts.Domain;
using AppointmentsOfADayReadModel = bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.AppointmentsOfADayReadModel;
using FixedAppointmentSet = bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.FixedAppointmentSet;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository
{
	public interface IClientReadModelRepository
	{					
		void RequestAppointmentsOfADayReadModel(Action<AppointmentsOfADayReadModel> readModelAvailable, 
												AggregateIdentifier id, Action<string> errorCallback);

		void RequestAppointmentSetOfADay(Action<FixedAppointmentSet> appointmentSetAvailable, 
										 AggregateIdentifier id, uint aggregateVersionLimit, Action<string> errorCallback);

		void RequestAppointmentsOfAPatientReadModel(Action<AppointmentsOfAPatientReadModel> readModelAvailable, 
													Guid patientId, Action<string> errorCallback);
	}
}