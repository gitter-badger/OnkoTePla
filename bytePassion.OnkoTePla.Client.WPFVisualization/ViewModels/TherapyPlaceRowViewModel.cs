using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class TherapyPlaceRowViewModel : ITherapyPlaceRowViewModel
	{

		private double timeSlotWidth;

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

		public double TimeSlotWidth {
			set
			{
				timeSlotWidth = value;

				RecomputeAppointmentPositions();

			}
			private get { return timeSlotWidth; }
		}

		public string TherapyPlaceName { get { return therapyPlace.Name; }}
		public Color  RoomColor        { get { return roomDisplayColor;  }}

		private void RecomputeAppointmentPositions()
		{
			
		}
	}
}
