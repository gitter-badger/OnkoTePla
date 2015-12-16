using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using System;
using System.Collections.Generic;
using System.Linq;


namespace bytePassion.OnkoTePla.Core.Domain.AppointmentLogic
{
    public class AppointmentSet
	{		
		private readonly IPatientReadRepository patientRepository;
		private readonly IConfigurationReadRepository configurationRepository;

		public AppointmentSet(IPatientReadRepository patientRepository, 
							  IConfigurationReadRepository configurationRepository)
		{
			this.patientRepository = patientRepository;
			this.configurationRepository = configurationRepository;
			
			ObservableAppointments = new ObservableAppointmentCollection();
		}

		public ObservableAppointmentCollection ObservableAppointments { get; }

		public IEnumerable<Appointment> AppointmentList
		{
			get { return ObservableAppointments.Appointments.ToList(); }
		} 

		public void AddAppointment(Guid medicalPracticeId, uint medicalPracticeVersion, CreateAppointmentData appointmentData)
		{
			var patient = patientRepository.GetPatientById(appointmentData.PatientId);
			var therapyPlace = configurationRepository.GetMedicalPracticeByIdAndVersion(medicalPracticeId, medicalPracticeVersion)
													  .GetTherapyPlaceById(appointmentData.TherapyPlaceId);

			var newAppointment = new Appointment(patient, appointmentData.Description, therapyPlace, 
												 appointmentData.Day, appointmentData.StartTime, appointmentData.EndTime, 
												 appointmentData.AppointmentId);

			ObservableAppointments.AddAppointment(newAppointment);			
		}

		public void DeleteAppointment(Guid removedAppointmentId)
		{
			ObservableAppointments.DeleteAppointment(removedAppointmentId);
		}

		public void ReplaceAppointment(Guid medicalPracticeId, uint medicalPracticeVersion, 
									   string newDescription, Date newDate,
								       Time newStartTime, Time newEndTime,
								       Guid newTherapyPlaceId,
								       Guid originalAppointmendId)
		{
			var appointmentToBeUpdated = ObservableAppointments.GetAppointmentById(originalAppointmendId);

			var newTherapyPlace = configurationRepository.GetMedicalPracticeByIdAndVersion(medicalPracticeId, medicalPracticeVersion)
													     .GetTherapyPlaceById(newTherapyPlaceId);

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
