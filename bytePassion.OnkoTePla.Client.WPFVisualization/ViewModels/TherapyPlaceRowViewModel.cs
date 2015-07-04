using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class TherapyPlaceRowViewModel : ITherapyPlaceRowViewModel
	{

		private double timeSlotWidth;
		private double lengthOfOneHour;

		private readonly TherapyPlace therapyPlace;
		private readonly ObservableCollection<IAppointmentViewModel> appointments;

		private readonly Color roomDisplayColor;

		private readonly Time startTime;
		private readonly Time endTime;

		public TherapyPlaceRowViewModel(ObservableCollection<IAppointmentViewModel> appointments,
										TherapyPlace therapyPlace, Color roomDisplayColor,
										Time startTime, Time endTime)
		{
			this.appointments = appointments;
			this.therapyPlace = therapyPlace;
			this.roomDisplayColor = roomDisplayColor;
			this.startTime = startTime;
			this.endTime = endTime;
		}

		public ObservableCollection<IAppointmentViewModel> Appointments { get { return appointments; }}

		public Time TimeSlotStart { get { return startTime; }}
		public Time TimeSlotEnd   { get { return endTime;   }}

		public double TimeSlotWidth {
			set
			{
				timeSlotWidth  = value;
				LengthOfOneHour = timeSlotWidth / (Time.GetDurationBetween(endTime, startTime).Seconds * 3600);				
			}
			private get { return timeSlotWidth; }
		}

		public string TherapyPlaceName { get { return therapyPlace.Name; }}

		public double LengthOfOneHour
		{
			get { return lengthOfOneHour; }
			private set { PropertyChanged.ChangeAndNotify(this, ref lengthOfOneHour, value); }
		}

		public Color  RoomColor        { get { return roomDisplayColor;  }}

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
