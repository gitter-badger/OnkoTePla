using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel
{
    public interface IAppointmentViewModelBuilder
	{
		IAppointmentViewModel Build(Appointment appointment, AggregateIdentifier location);
	}
}