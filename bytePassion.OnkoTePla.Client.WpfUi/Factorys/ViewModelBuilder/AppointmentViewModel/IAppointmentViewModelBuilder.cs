using System;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel
{
	internal interface IAppointmentViewModelBuilder
	{
		IAppointmentViewModel Build (Appointment appointment, AggregateIdentifier location, Action<string> errorCallback);
	}
}