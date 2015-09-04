using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public class TherapyPlaceRowViewModelSampleData : ITherapyPlaceRowViewModel
	{
		public TherapyPlaceRowViewModelSampleData()
		{
			TherapyPlaceName = "place 1";
			RoomColor = Colors.LightBlue;						

			AppointmentViewModels = new ObservableCollection<IAppointmentViewModel>
			{
				new AppointmentViewModelSampleData( 10, 150),
				new AppointmentViewModelSampleData(200, 150)
			};			
		}
		
		public ObservableCollection<IAppointmentViewModel> AppointmentViewModels { get; }			

		public string TherapyPlaceName { get; }
		public Color  RoomColor        { get; }
			
	}
}