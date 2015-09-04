using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Messages
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
