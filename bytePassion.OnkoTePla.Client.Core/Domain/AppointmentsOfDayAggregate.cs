using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Exceptions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public class AppointmentsOfDayAggregate : AggregateRootBase<AggregateIdentifier>
	{

		private readonly AppointmentSet appointments;		

		public AppointmentsOfDayAggregate(AggregateIdentifier id, 
										  IPatientReadRepository patientRepository, 
										  IConfigurationReadRepository config) 
			: base(id, 0)
		{			
			appointments = new AppointmentSet(patientRepository, config);
		}

		public void Apply (AppointmentAdded @event)
		{
			if (Version + 1 != @event.AggregateVersion)
				throw new VersionNotApplicapleException("@apply AppointmentAdded");

			appointments.AddAppointment(@event.AggregateId.MedicalPracticeId, 
										@event.AggregateId.PracticeVersion, 
										@event.CreateAppointmentData);

			Version = @event.AggregateVersion;
		}

		public void Apply(AppointmentModified @event)
		{			
		}

		public void Apply(AppointmentRemoved @event)
		{			
		}
		
		public void AddAppointment(Guid userId, uint forAggregateVersion, CreateAppointmentData createAppointmentData)
		{

			if (forAggregateVersion != Version)
				throw new VersionNotApplicapleException("@addAppointmen");
			
			var newAggregateVersion = Version + 1;

 			ApplyChange(new AppointmentAdded(Id, newAggregateVersion, 
											 userId,TimeTools.GetCurrentTimeStamp(), 
											 createAppointmentData));
		}
	}
}
