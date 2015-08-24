using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public interface ITherapyPlaceRowViewModel
	{
		AppointmentLocalisation LocalisationIdentifier { get; }

		ObservableCollection<IAppointmentViewModel> Appointments { get; }		

		Time TimeSlotStart { get; } 
		Time TimeSlotEnd   { get; }
		
		string TherapyPlaceName { get; }
		Color  RoomColor        { get; }			
		
	}
}