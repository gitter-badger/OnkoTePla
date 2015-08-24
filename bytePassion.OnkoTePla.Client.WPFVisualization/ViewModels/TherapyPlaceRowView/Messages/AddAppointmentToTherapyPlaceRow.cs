using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Messages
{
	public class AddAppointmentToTherapyPlaceRow : ViewModelMessageBase
	{
		public AddAppointmentToTherapyPlaceRow(AppointmentViewModel appointmentViewModelToAdd)
		{
			AppointmentViewModelToAdd = appointmentViewModelToAdd;			
		}

		public AppointmentViewModel AppointmentViewModelToAdd { get; }		
	}
}
