using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic
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

		public void ReplaceAppointment(Guid medicalPracticeId, uint medicalPracticeVersion, ReplaceAppointmentData replaceAppointmentData)
		{
			var appointmentToBeUpdated = ObservableAppointments.GetAppointmentById(replaceAppointmentData.OriginalAppointmendId);

			var newTherapyPlace = configurationRepository.GetMedicalPracticeByIdAndVersion(medicalPracticeId, medicalPracticeVersion)
													     .GetTherapyPlaceById(replaceAppointmentData.NewTherapyPlaceId);

			var updatedAppointment = new Appointment(appointmentToBeUpdated.Patient,
													 replaceAppointmentData.NewDescription,
													 newTherapyPlace,
													 replaceAppointmentData.NewDate,
													 replaceAppointmentData.NewStartTime,
													 replaceAppointmentData.NewEndTime,
													 replaceAppointmentData.OriginalAppointmendId);

			ObservableAppointments.ReplaceAppointment(updatedAppointment);
		}
	}	
}
