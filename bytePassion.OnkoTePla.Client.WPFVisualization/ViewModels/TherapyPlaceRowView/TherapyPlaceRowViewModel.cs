using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public class TherapyPlaceRowViewModel : ITherapyPlaceRowViewModel
	{
		public TherapyPlaceRowViewModel(TherapyPlace therapyPlace, Color roomDisplayColor,
										Time startTime, Time endTime)
		{			
			RoomColor = roomDisplayColor;
			TimeSlotStart = startTime;
			TimeSlotEnd = endTime;
			TherapyPlaceId = therapyPlace.Id;
			TherapyPlaceName = therapyPlace.Name;

			Appointments = new ObservableCollection<IAppointmentViewModel>();			
		}

		public ObservableCollection<IAppointmentViewModel> Appointments { get; }

		public Time   TimeSlotStart    { get; }
		public Time   TimeSlotEnd      { get; }
		public Color  RoomColor        { get; }
		public Guid   TherapyPlaceId   { get; }
		public string TherapyPlaceName { get; }

		public void AddAppointment(IAppointmentViewModel newAppointment)
		{
			Appointments.Add(newAppointment);
		}

		public void RemoveAppointment(IAppointmentViewModel appointmentToRemove)
		{
			Appointments.Remove(appointmentToRemove);
		}				
	}
}
