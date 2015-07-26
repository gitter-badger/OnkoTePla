using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewModel;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowViewModel
{
	public class TherapyPlaceRowViewModel : ITherapyPlaceRowViewModel
	{
			
		private readonly TherapyPlace therapyPlace;
		private readonly ObservableCollection<IAppointmentViewModel> appointments;

		public TherapyPlaceRowViewModel(TherapyPlace therapyPlace, Color roomDisplayColor,
										Time startTime, Time endTime)
		{			
			this.therapyPlace = therapyPlace;
			RoomColor = roomDisplayColor;
			TimeSlotStart = startTime;
			TimeSlotEnd = endTime;

			appointments = new ObservableCollection<IAppointmentViewModel>();			
		}

		public ObservableCollection<IAppointmentViewModel> Appointments
		{
			get { return appointments; }
		}

		public Time TimeSlotStart { get; }
		public Time TimeSlotEnd   { get; }		

		public void AddAppointment(IAppointmentViewModel newAppointment)
		{
			appointments.Add(newAppointment);
		}

		public void RemoveAppointment(IAppointmentViewModel appointmentToRemove)
		{
			appointments.Remove(appointmentToRemove);
		}
	
		public Color  RoomColor { get; }

		public Guid   TherapyPlaceId   => therapyPlace.Id;
		public string TherapyPlaceName => therapyPlace.Name;	
	}
}
