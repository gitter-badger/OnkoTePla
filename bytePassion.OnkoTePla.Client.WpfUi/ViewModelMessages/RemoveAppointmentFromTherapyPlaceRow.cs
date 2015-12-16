using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages
{
    public class RemoveAppointmentFromTherapyPlaceRow : ViewModelMessage
	{
		public RemoveAppointmentFromTherapyPlaceRow (AppointmentViewModel appointmentViewModelToAdd)
		{
			AppointmentViewModelToRemove = appointmentViewModelToAdd;			
		}

		public AppointmentViewModel AppointmentViewModelToRemove { get; }		
	}
}
