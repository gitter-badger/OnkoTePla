﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface ITherapyPlaceRowViewModel : IViewModelBase
	{
		ObservableCollection<IAppointmentViewModel> Appointments { get; }

		Time TimeSlotStart { get; } 
		Time TimeSlotEnd   { get; }
		
		string TherapyPlaceName { get; }
		Color  RoomColor        { get; }

		double LengthOfOneHour { get; }		
		Guid   TherapyPlaceId  { get; }

		double TimeSlotWidth { set; }

		void AddAppointment   (IAppointmentViewModel newAppointment);
		void RemoveAppointment(IAppointmentViewModel appointmentToRemove);
	}
}