using System;
using System.Collections.Generic;
using System.Windows;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;

namespace bytePassion.OnkoTePla.Client.DataAndService.Readmodels
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
				AddAppointment(new CreateAppointmentData(appointmentTransferData.PatientId,
														 appointmentTransferData.Description,
														 appointmentTransferData.StartTime, 
														 appointmentTransferData.EndTime,
														 appointmentTransferData.Day,
														 appointmentTransferData.TherapyPlaceId,
														 appointmentTransferData.Id), 
							   errorCallback);
			}
		}

		public ObservableAppointmentCollection ObservableAppointments { get; }

		public IEnumerable<Appointment> AppointmentList
		{
			get { return ObservableAppointments.Appointments; }
		} 		

		public void AddAppointment(CreateAppointmentData appointmentData, Action<string> errorCallback)
		{	
			patientRepository.RequestPatient(
				patient =>
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						var newAppointment = new Appointment(patient,
															 appointmentData.Description,
															 medicalPractice.GetTherapyPlaceById(appointmentData.TherapyPlaceId),
															 appointmentData.Day,
															 appointmentData.StartTime,
															 appointmentData.EndTime,
															 appointmentData.AppointmentId);

						ObservableAppointments.AddAppointment(newAppointment);
					});								
				},
				appointmentData.PatientId,
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
