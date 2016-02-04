using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Core.Domain.AppointmentLogic
{
	public class AppointmentSet
	{		
		private readonly IPatientReadRepository patientRepository;
		
		public AppointmentSet(IPatientReadRepository patientRepository)
			: this(patientRepository, new ObservableAppointmentCollection())
		{			
		}

		public AppointmentSet(IPatientReadRepository patientRepository,
							  ObservableAppointmentCollection appointmentCollection)
		{
			this.patientRepository = patientRepository;
			ObservableAppointments = appointmentCollection;
		}

		public ObservableAppointmentCollection ObservableAppointments { get; }

		public IEnumerable<Appointment> AppointmentList
		{
			get { return ObservableAppointments.Appointments; }
		} 

		public void AddAppointment(CreateAppointmentData appointmentData, 
								   ClientMedicalPracticeData medicalPractice)
		{
			var patient = patientRepository.GetPatientById(appointmentData.PatientId);
			var therapyPlace = medicalPractice.GetTherapyPlaceById(appointmentData.TherapyPlaceId);

			var newAppointment = new Appointment(patient, appointmentData.Description, therapyPlace, 
												 appointmentData.Day, appointmentData.StartTime, appointmentData.EndTime, 
												 appointmentData.AppointmentId);

			ObservableAppointments.AddAppointment(newAppointment);			
		}

		public void DeleteAppointment(Guid removedAppointmentId)
		{
			ObservableAppointments.DeleteAppointment(removedAppointmentId);
		}
		
		public void ReplaceAppointment(string newDescription, Date newDate,
								       Time newStartTime, Time newEndTime,
								       Guid newTherapyPlaceId,
								       Guid originalAppointmendId,
									   ClientMedicalPracticeData medicalPractice)
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
