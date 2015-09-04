using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid
{
	public class TimeGridViewModelSampleData : ITimeGridViewModel
	{
		public TimeGridViewModelSampleData()
		{
			TimeSlotLabels = new ObservableCollection<TimeSlotLabel>
			{
				new TimeSlotLabel("8:00") { XCoord = 200, YCoord = 10 },
				new TimeSlotLabel("7:00") { XCoord = 100, YCoord = 10 }
			};

			TimeSlotLines = new ObservableCollection<TimeSlotLine>
			{
				new TimeSlotLine {XCoord =  100, YCoordTop = 40, YCoordBottom = 400},
				new TimeSlotLine {XCoord =  200, YCoordTop = 40, YCoordBottom = 400}
			};
		}

		public ObservableCollection<TimeSlotLabel> TimeSlotLabels { get; }
		public ObservableCollection<TimeSlotLine>  TimeSlotLines  { get; }
	}
}
