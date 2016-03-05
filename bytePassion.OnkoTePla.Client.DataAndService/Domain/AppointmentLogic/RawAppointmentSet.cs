using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.AppointmentLogic
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

		public void AddAppointment (Guid patientId, string description,
									Time startTime, Time endTime, Date day, 
									Guid therapyPlaceId, Guid appointmentId,
									Guid medicalPracticeId)
		{
			var newAppointment = new AppointmentTransferData(patientId,
															 description,							
															 day,
															 startTime,
															 endTime,
															 therapyPlaceId,
															 appointmentId,
															 medicalPracticeId);
			
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
																 originalAppointmendId,
																 appointmentToBeUpdated.MedicalPracticeId);

			ObservableAppointments.ReplaceAppointment(updatedAppointment);
		}
	}
}