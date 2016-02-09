using System;
using bytePassion.OnkoTePla.Core.Domain;
using AppointmentsOfADayReadModel = bytePassion.OnkoTePla.Client.DataAndService.Readmodels.AppointmentsOfADayReadModel;
using FixedAppointmentSet = bytePassion.OnkoTePla.Client.DataAndService.Readmodels.FixedAppointmentSet;

namespace bytePassion.OnkoTePla.Client.DataAndService.ReadModelRepository
{
	public interface IClientReadModelRepository
	{					
		void RequestAppointmentsOfADayReadModel(Action<AppointmentsOfADayReadModel> readModelAvailable, 
												AggregateIdentifier id, Action<string> errorCallback);

		void RequestAppointmentSetOfADay(Action<FixedAppointmentSet> appointmentSetAvailable, 
										 AggregateIdentifier id, uint aggregateVersionLimit, Action<string> errorCallback);
	}
}