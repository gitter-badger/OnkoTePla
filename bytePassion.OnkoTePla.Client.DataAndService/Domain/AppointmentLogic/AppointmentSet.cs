using System;
using System.Collections.Generic;
using System.Windows;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.LabelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.AppointmentLogic
{
	public class AppointmentSet
	{		
		private readonly IClientPatientRepository patientRepository;
		private readonly IClientLabelRepository labelRepository;
		private readonly ClientMedicalPracticeData medicalPractice;		

		public AppointmentSet(IClientPatientRepository patientRepository,
							  IClientLabelRepository labelRepository,							  
							  IEnumerable<AppointmentTransferData> initialAppointmentData,
							  ClientMedicalPracticeData medicalPractice,							  
							  Action<string> errorCallback)
		{
			this.patientRepository = patientRepository;
			this.labelRepository = labelRepository;
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
							   appointmentTransferData.LabelId,
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
								   Guid labelId,
								   Action<string> errorCallback)
		{
			patientRepository.RequestPatient(
				patient =>
				{
					labelRepository.RequestLabel(
						label =>
						{
							Application.Current.Dispatcher.Invoke(() =>
							{
								var newAppointment = new Appointment(patient,
															 description,
															 medicalPractice.GetTherapyPlaceById(therapyPlaceId),
															 day,
															 startTime,
															 endTime,
															 appointmentId,
															 label);

								ObservableAppointments.AddAppointment(newAppointment);
							});							
						},
						labelId,
						errorCallback						
					);							
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
								       Guid newTherapyPlaceId, Guid newLabelId,
								       Guid originalAppointmendId,
									   Action<string> errorCallback)
		{
			var appointmentToBeUpdated = ObservableAppointments.GetAppointmentById(originalAppointmendId);
			
			var newTherapyPlace = medicalPractice.GetTherapyPlaceById(newTherapyPlaceId);

			labelRepository.RequestLabel(
				label =>
				{
					var updatedAppointment = new Appointment(appointmentToBeUpdated.Patient,
															 newDescription,
															 newTherapyPlace,
															 newDate,
															 newStartTime,
															 newEndTime,
															 originalAppointmendId,
															 label);

					ObservableAppointments.ReplaceAppointment(updatedAppointment);
				},
				newLabelId,
				errorCallback	
			);			
		}
	}
}
