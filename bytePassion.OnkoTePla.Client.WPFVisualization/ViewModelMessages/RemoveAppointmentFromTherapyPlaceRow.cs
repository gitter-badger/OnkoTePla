using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
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
