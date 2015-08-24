using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public class TherapyPlaceRowViewModelSampleData : ITherapyPlaceRowViewModel
	{
		public TherapyPlaceRowViewModelSampleData()
		{
			TherapyPlaceName = "place 1";
			RoomColor = Colors.LightBlue;
			
			TimeSlotStart = new Time(7,0);
			TimeSlotEnd   = new Time(16,0);			

			Appointments = new ObservableCollection<IAppointmentViewModel>
			{
				new AppointmentViewModelSampleData( 10, 150),
				new AppointmentViewModelSampleData(200, 150)
			};

			LocalisationIdentifier = new AppointmentLocalisation(
				new AggregateIdentifier(Date.Dummy, new Guid()), 
				new Guid()
			);
		}

		public AppointmentLocalisation LocalisationIdentifier { get; }

		public ObservableCollection<IAppointmentViewModel> Appointments { get; }
		

		public Time TimeSlotStart { get; }
		public Time TimeSlotEnd   { get; }

		public string TherapyPlaceName { get; }
		public Color  RoomColor        { get; }
			
	}
}