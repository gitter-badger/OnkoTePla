using System;
using System.Collections.Generic;
using System.Windows;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.AppointmentLogic
{
	public class AppointmentSet
	{		
		private readonly IClientPatientRepository patientRepository;
		private readonly ClientMedicalPracticeData medicalPractice;		

		public AppointmentSet(IClientPatientRepository patientRepository,							  
							  IEnumerable<AppointmentTransferData> initialAppointmentData,
							  ClientMedicalPracticeData medicalPractice,							  
							  Action<string> errorCallback)
		{
			this.patientRepository = patientRepository;
			this.medicalPractice = medicalPractice;			 

			ObservableAppointments = new ObservableAppointmentCollection();
			
			foreach (var appointmentTransferData in initialAppointmentData)
			{
				AddAppointment(appointmentTransferData.PatientId,
							   appointmentTransferData.Description,
							   appointmentTransferData.StartTime, 
							   appointmentTransferData.EndTime,
							   appointmentTransferData.Day,
							   appointmentTransferData.TherapyPlaceId,
							   appointmentTransferData.Id, 
							   errorCallback);
			}
		}

		public ObservableAppointmentCollection ObservableAppointments { get; }

		public IEnumerable<Appointment> AppointmentList
		{
			get { return ObservableAppointments.Appointments; }
		} 		
		
		public void AddAppointment(Guid patientId, string description,
								   Time startTime, Time endTime, Date day, 
								   Guid therapyPlaceId, Guid appointmentId, 
								   Action<string> errorCallback)
		{
			patientRepository.RequestPatient(
				patient =>
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						var newAppointment = new Appointment(patient,
															 description,
															 medicalPractice.GetTherapyPlaceById(therapyPlaceId),
															 day,
															 startTime,
															 endTime,
															 appointmentId);

						ObservableAppointments.AddAppointment(newAppointment);
					});								
				},
				patientId,
				errorCallback	
			);								
		}						

		public void DeleteAppointment(Guid removedAppointmentId)
		{
			ObservableAppointments.DeleteAppointment(removedAppointmentId);
		}
		
		public void ReplaceAppointment(string newDescription, Date newDate,
								       Time newStartTime, Time newEndTime,
								       Guid newTherapyPlaceId,
								       Guid originalAppointmendId)
		{
			var appointmentToBeUpdated = ObservableAppointments.GetAppointmentById(originalAppointmendId);
			
			var newTherapyPlace = medicalPractice.GetTherapyPlaceById(newTherapyPlaceId);

			var updatedAppointment = new Appointment(appointmentToBeUpdated.Patient,
													 newDescription,
													 newTherapyPlace,
													 newDate,
													 newStartTime,
													 newEndTime,
													 originalAppointmendId);

			ObservableAppointments.ReplaceAppointment(updatedAppointment);
		}
	}
}
