using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public interface ITherapyPlaceRowViewModel
	{		
		ObservableCollection<IAppointmentViewModel> AppointmentViewModels { get; }				
		
		string TherapyPlaceName { get; }
		Color  RoomColor        { get; }					
	}
}