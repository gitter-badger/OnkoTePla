using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowViewModel
{
	public class TherapyPlaceRowViewModelSampleData : ITherapyPlaceRowViewModel
	{
		public TherapyPlaceRowViewModelSampleData()
		{
			TherapyPlaceName = "place 1";
			RoomColor = Colors.LightBlue;
			
			TimeSlotStart = new Time(7,0);
			TimeSlotEnd   = new Time(16,0);
			TherapyPlaceId = new Guid();

			Appointments = new ObservableCollection<IAppointmentViewModel>
			{
				new AppointmentViewModelSampleData( 10, 150),
				new AppointmentViewModelSampleData(200, 150)
			};
		}

		public ObservableCollection<IAppointmentViewModel> Appointments { get; }

		public Time TimeSlotStart { get; }
		public Time TimeSlotEnd   { get; }

		public string TherapyPlaceName { get; }
		public Color  RoomColor        { get; }
		
		public Guid   TherapyPlaceId  { get; }		

		public void AddAppointment   (IAppointmentViewModel newAppointment)      {}
		public void RemoveAppointment(IAppointmentViewModel appointmentToRemove) {}		

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}