using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	public class AppointmentGridViewModelSampleData : IAppointmentGridViewModel
	{
		public AppointmentGridViewModelSampleData()
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

			TherapyPlaceRows = new ObservableCollection<ITherapyPlaceRowViewModel>
			{
				new TherapyPlaceRowViewModelSampleData(),
				new TherapyPlaceRowViewModelSampleData(),
				new TherapyPlaceRowViewModelSampleData(),
			};

			CurrentGridWidth  = 800;
			CurrentGridHeight = 400;

			OperatingMode = AppointmentGridViewMode.View;
		}

		public ICommand LoadReadModel { get { return null; }}

		public ObservableCollection<TimeSlotLabel> TimeSlotLabels { get; private set; }
		public ObservableCollection<TimeSlotLine> TimeSlotLines   { get; private set; }

		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRows { get; private set; }

		public double CurrentGridWidth  { set; get; }
		public double CurrentGridHeight { set; get; }

		public IAppointmentViewModel   EditingObject { set; get; }
		public AppointmentGridViewMode OperatingMode { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
