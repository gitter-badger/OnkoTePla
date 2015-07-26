using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewModel;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowViewModel
{
	public interface ITherapyPlaceRowViewModel
	{
		ObservableCollection<IAppointmentViewModel> Appointments { get; }

		Time TimeSlotStart { get; } 
		Time TimeSlotEnd   { get; }
		
		string TherapyPlaceName { get; }
		Color  RoomColor        { get; }
			
		Guid   TherapyPlaceId  { get; }		

		void AddAppointment   (IAppointmentViewModel newAppointment);
		void RemoveAppointment(IAppointmentViewModel appointmentToRemove);
	}
}