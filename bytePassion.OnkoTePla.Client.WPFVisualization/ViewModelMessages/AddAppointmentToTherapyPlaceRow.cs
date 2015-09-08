using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
{
	public class AddAppointmentToTherapyPlaceRow : ViewModelMessage
	{
		public AddAppointmentToTherapyPlaceRow(AppointmentViewModel appointmentViewModelToAdd)
		{
			AppointmentViewModelToAdd = appointmentViewModelToAdd;			
		}

		public AppointmentViewModel AppointmentViewModelToAdd { get; }		
	}
}
