using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;

namespace bytePassion.OnkoTePla.Client.DataAndService.Readmodels
{
	public class RawAppointmentSet
	{		
		public RawAppointmentSet (IEnumerable<AppointmentTransferData> initialAppointmentData)
		{			
			ObservableAppointments = new ObservableRawAppointmentCollection(initialAppointmentData);			
		}

		public ObservableRawAppointmentCollection ObservableAppointments { get; }

		public IEnumerable<AppointmentTransferData> AppointmentList
		{
			get { return ObservableAppointments.Appointments; }
		}

		public void AddAppointment (CreateAppointmentData appointmentData)
		{			
			var newAppointment = new AppointmentTransferData(appointmentData.PatientId,
															 appointmentData.Description,							
															 appointmentData.Day,
															 appointmentData.StartTime,
															 appointmentData.EndTime,
															 appointmentData.TherapyPlaceId,
															 appointmentData.AppointmentId);

			ObservableAppointments.AddAppointment(newAppointment);				
		}

		public void DeleteAppointment (Guid removedAppointmentId)
		{
			ObservableAppointments.DeleteAppointment(removedAppointmentId);
		}

		public void ReplaceAppointment (string newDescription, Date newDate,
									    Time newStartTime, Time newEndTime,
									    Guid newTherapyPlaceId,
									    Guid originalAppointmendId)
		{
			var appointmentToBeUpdated = ObservableAppointments.GetAppointmentById(originalAppointmendId);
			

			var updatedAppointment = new AppointmentTransferData(appointmentToBeUpdated.PatientId,
																 newDescription,																 
																 newDate,
																 newStartTime,
																 newEndTime,
																 newTherapyPlaceId,
																 originalAppointmendId);

			ObservableAppointments.ReplaceAppointment(updatedAppointment);
		}
	}
}