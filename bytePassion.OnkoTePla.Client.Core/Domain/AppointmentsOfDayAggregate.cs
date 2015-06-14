using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public class AppointmentsOfDayAggregate : AggregateRootBase<AggregateIdentifier>
	{

		private readonly IList<Appointment> appointments;
		private readonly IPatientReadRepository patientRepository;
		private readonly IConfigurationRepository config;

		public AppointmentsOfDayAggregate(AggregateIdentifier id, 
										  IPatientReadRepository patientRepository, 
										  IConfigurationRepository config) 
			: base(id, 0)
		{
			this.patientRepository = patientRepository;
			this.config = config;
			appointments = new List<Appointment>();
		}

		public void Apply (AppointmentAdded @event)
		{
			var medicalPractice = config.GetMedicalPracticeByIdAndVersion(Id.MedicalPracticeId, Id.PracticeVersion);

			appointments.Add(new Appointment(patientRepository.GetPatientById(@event.PatientId), 
											 medicalPractice.GetTherapyPlaceById(@event.TherapyPlaceId), 
											 medicalPractice.GetRoomById(@event.RoomId), 
											 @event.Day, @event.StartTime, @event.EndTime));
		}

		public void Apply(AppointmentModified @event)
		{			
		}

		public void Apply(AppointmentRemoved @event)
		{			
		}
		
		public void AddAppointment(Guid userId,
								   Guid patientId, string description, 
								   Time startTime, Time endTime,
								   Guid therapyPlaceId, Guid roomId) 
		{
 			ApplyChange(new AppointmentAdded(Id, Version, userId,TimeTools.GetCurrentTimeStamp(), 
											 patientId, description, startTime, endTime, 
											 therapyPlaceId, roomId ));
		}
	}
}
