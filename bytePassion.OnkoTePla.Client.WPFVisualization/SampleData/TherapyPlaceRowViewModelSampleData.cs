using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	public class TherapyPlaceRowViewModelSampleData : ITherapyPlaceRowViewModel
	{
		public TherapyPlaceRowViewModelSampleData()
		{
			TherapyPlaceName = "place 1";
			RoomColor = Colors.LightBlue;
			LengthOfOneHour = 200;
			TimeSlotStart = new Time(7,0);
			TimeSlotEnd   = new Time(16,0);

			Appointments = new ObservableCollection<IAppointmentViewModel>
			{
				new AppointmentViewModelSampleData(new Time(8,0),new Time(10,0)),
				new AppointmentViewModelSampleData(new Time(8,0),new Time(10,0))
			};
		}

		public ObservableCollection<IAppointmentViewModel> Appointments { get; private set; }

		public Time TimeSlotStart { get; private set; }
		public Time TimeSlotEnd   { get; private set; }

		public double TimeSlotWidth { set {} }

		public string TherapyPlaceName { get; private set; }
		public double LengthOfOneHour  { get; private set; }

		public Color RoomColor { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}