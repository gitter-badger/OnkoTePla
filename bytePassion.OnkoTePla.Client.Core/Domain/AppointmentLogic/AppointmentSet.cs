using System;
using bytePassion.Lib.TimeLib;
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

		public ObservableAppointmentCollection Appointments
		{
			get { return appointmentCollection; }
		}

		public void AddAppointment(Guid patientId, string description, 
								   Guid medicalPracticeId, uint medicalPracticeVersion, 
								   Guid therapyPlaceId, Date day, 
								   Time startTime, Time endTime, Guid appointmentId)
		{
			var patient = patientRepository.GetPatientById(patientId);
			var therapyPlace = configurationRepository.GetMedicalPracticeByIdAndVersion(medicalPracticeId, medicalPracticeVersion)
													  .GetTherapyPlaceById(therapyPlaceId);

			var newAppointment = new Appointment(patient, description, therapyPlace, day, startTime, endTime, appointmentId);

			appointmentCollection.AddAppointment(newAppointment);
			//appointmentLookUp.Add(appointmentId, newAppointment);
		}
	}	
}
