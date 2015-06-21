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
		//private readonly IDictionary<Guid, Appointment>  appointmentLookUp;
		private readonly ObservableAppointmentCollection appointmentCollection;

		private readonly IPatientReadRepository patientRepository;
		private readonly IConfigurationReadRepository configurationRepository;

		public AppointmentSet(IPatientReadRepository patientRepository, 
							  IConfigurationReadRepository configurationRepository)
		{
			this.patientRepository = patientRepository;
			this.configurationRepository = configurationRepository;

			//appointmentLookUp = new Dictionary<Guid, Appointment>();
			appointmentCollection = new ObservableAppointmentCollection();
		}

		public ObservableAppointmentCollection ObservableAppointments
		{
			get { return appointmentCollection; }
		}

		public IEnumerable<Appointment> AppointmentList
		{
			get { return appointmentCollection.Appointments.ToList(); }
		} 

		public void AddAppointment(Guid medicalPracticeId, uint medicalPracticeVersion, CreateAppointmentData appointmentData)
		{
			var patient = patientRepository.GetPatientById(appointmentData.PatientId);
			var therapyPlace = configurationRepository.GetMedicalPracticeByIdAndVersion(medicalPracticeId, medicalPracticeVersion)
													  .GetTherapyPlaceById(appointmentData.TherapyPlaceId);

			var newAppointment = new Appointment(patient, appointmentData.Description, therapyPlace, 
												 appointmentData.Day, appointmentData.StartTime, appointmentData.EndTime, 
												 appointmentData.AppointmentId);

			appointmentCollection.AddAppointment(newAppointment);
			//appointmentLookUp.Add(appointmentId, newAppointment);
		}
	}	
}
