using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Exceptions;
using bytePassion.OnkoTePla.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Core.Domain
{
	public class AppointmentsOfDayAggregate : AggregateRootBase<AggregateIdentifier>
	{
		private readonly ClientMedicalPracticeData medicalPractice;
		private readonly AppointmentSet appointments;		

		public AppointmentsOfDayAggregate(AggregateIdentifier id, 
										  IPatientReadRepository patientRepository, 
										  ClientMedicalPracticeData medicalPractice) 
			: base(id, 0)
		{
			this.medicalPractice = medicalPractice;
			appointments = new AppointmentSet(patientRepository);
		}

		public void Apply (AppointmentAdded @event)
		{
			if (Version + 1 != @event.AggregateVersion)
				throw new VersionNotApplicapleException("@apply AppointmentAdded");

			appointments.AddAppointment(@event.CreateAppointmentData, medicalPractice);

			Version = @event.AggregateVersion;
		}

		public void Apply(AppointmentReplaced @event)
		{
			if (Version + 1 != @event.AggregateVersion)
				throw new VersionNotApplicapleException("@apply AppointmentReplaced");

			appointments.ReplaceAppointment(@event.NewDescription, 
											@event.NewDate, 
											@event.NewStartTime, 
											@event.NewEndTime, 
											@event.NewTherapyPlaceId, 
											@event.OriginalAppointmendId,
											medicalPractice);

			Version = @event.AggregateVersion;
		}

		public void Apply(AppointmentDeleted @event)
		{
			if (Version + 1 != @event.AggregateVersion)
				throw new VersionNotApplicapleException("@apply AppointmentDeleted");

			appointments.DeleteAppointment(@event.RemovedAppointmentId);

			Version = @event.AggregateVersion;
		}
		
		public void AddAppointment(Guid userId, uint forAggregateVersion, ActionTag actionTag, 
								   CreateAppointmentData createAppointmentData)
		{

			if (forAggregateVersion != Version)
				throw new VersionNotApplicapleException("@addAppointmen");
			
			var newAggregateVersion = Version + 1;

 			ApplyChange(new AppointmentAdded(Id, newAggregateVersion, 
											 userId,TimeTools.GetCurrentTimeStamp(), 
											 actionTag,
											 createAppointmentData)); 
		}

		public void ReplaceAppointemnt(Guid userId, uint forAggregateVersion, 
									   Guid patientId, ActionTag actionTag,
									   string newDescription, Date newDate,
								       Time newStartTime, Time newEndTime,
								       Guid newTherapyPlaceId,
								       Guid originalAppointmendId)
		{
			if (forAggregateVersion != Version)
				throw new VersionNotApplicapleException("@replaceAppointment");

			var newAggregateVersion = Version + 1;

			ApplyChange(new AppointmentReplaced(Id, newAggregateVersion, 
												userId, patientId, 
												TimeTools.GetCurrentTimeStamp(),
												actionTag,
												newDescription, newDate,
												newStartTime, newEndTime,
												newTherapyPlaceId,
												originalAppointmendId));
		}

		public void DeleteAppointment(Guid userId, uint forAggregateVersion, Guid patientId, 
									  ActionTag actionTag, Guid appointmentToRemoveId)
		{
			if (forAggregateVersion != Version)
				throw new VersionNotApplicapleException("@deleteAppointmen");

			var newAggregateVersion = Version + 1;

			ApplyChange(new AppointmentDeleted(Id, newAggregateVersion, 
											   userId, patientId, 
											   TimeTools.GetCurrentTimeStamp(),
											   actionTag,
											   appointmentToRemoveId));
		}
	}
}
