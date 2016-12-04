using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages
{
	internal class AddAppointmentToTherapyPlaceRow : ViewModelMessage
	{
		public AddAppointmentToTherapyPlaceRow(AppointmentViewModel appointmentViewModelToAdd)
		{
			AppointmentViewModelToAdd = appointmentViewModelToAdd;			
		}

		public AppointmentViewModel AppointmentViewModelToAdd { get; }		
	}
}
