using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;

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

			OperatingMode = OperatingMode.View;
		}

		public ICommand LoadReadModel  { get { return null; }}
		public ICommand CommitChanges  { get { return null; }}
		public ICommand DiscardChanges { get { return null; }}

		public void DeleteAppointment(IAppointmentViewModel appointmentViewModel, Appointment appointment, ITherapyPlaceRowViewModel containerRow) {}
		
		public ObservableCollection<TimeSlotLabel> TimeSlotLabels { get; private set; }
		public ObservableCollection<TimeSlotLine> TimeSlotLines   { get; private set; }

		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRows { get; private set; }

		public double CurrentGridWidth  { set; get; }
		public double CurrentGridHeight { set; get; }

		public IAppointmentViewModel EditingObject { set; get; }
		public OperatingMode         OperatingMode { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
