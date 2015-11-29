using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder.AppointmentViewModel
{
	public interface IAppointmentViewModelBuilder
	{
		IAppointmentViewModel Build(Appointment appointment, AggregateIdentifier location);
	}
}