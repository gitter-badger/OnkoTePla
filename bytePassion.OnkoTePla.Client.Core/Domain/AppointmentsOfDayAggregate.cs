using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Client.Core.Exceptions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public class AppointmentsOfDayAggregate : AggregateRootBase<AggregateIdentifier>
	{

		private readonly IList<Appointment> appointments;
		private readonly IPatientReadRepository patientRepository;
		private readonly IConfigurationReadRepository config;

		public AppointmentsOfDayAggregate(AggregateIdentifier id, 
										  IPatientReadRepository patientRepository, 
										  IConfigurationReadRepository config) 
			: base(id, 0)
		{
			this.patientRepository = patientRepository;
			this.config = config;
			appointments = new List<Appointment>();
		}

		public void Apply (AppointmentAdded @event)
		{
			if (Version + 1 != @event.AggregateVersion)
				throw new VersionNotApplicapleException("@apply AppointmentAdded");

			var medicalPractice = config.GetMedicalPracticeByIdAndVersion(Id.MedicalPracticeId, Id.PracticeVersion);

			appointments.Add(new Appointment(patientRepository.GetPatientById(@event.PatientId), 
											 medicalPractice.GetTherapyPlaceById(@event.TherapyPlaceId), 
											 medicalPractice.GetRoomById(@event.RoomId), 
											 @event.Day, @event.StartTime, @event.EndTime));

			Version = @event.AggregateVersion;
		}

		public void Apply(AppointmentModified @event)
		{			
		}

		public void Apply(AppointmentRemoved @event)
		{			
		}
		
		public void AddAppointment(Guid userId, uint forAggregateVersion,
								   Guid patientId, string description, 
								   Time startTime, Time endTime,
								   Guid therapyPlaceId, Guid roomId)
		{

			if (forAggregateVersion != Version)
				throw new VersionNotApplicapleException("@addAppointmen");

			var newAggregateVersion = Version + 1;

 			ApplyChange(new AppointmentAdded(Id, newAggregateVersion, userId,TimeTools.GetCurrentTimeStamp(), 
											 patientId, description, startTime, endTime, 
											 therapyPlaceId, roomId ));
		}
	}
}
