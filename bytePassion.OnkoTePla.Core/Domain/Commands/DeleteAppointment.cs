using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Eventsystem;
using System;


namespace bytePassion.OnkoTePla.Core.Domain.Commands
{
    public class DeleteAppointment : DomainCommand
	{
		public DeleteAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, 
								 Guid userId, Guid patientId, ActionTag actionTag, 
								 Guid appointmentToRemoveId)
			: base(userId, patientId, actionTag)
		{
			AppointmentToRemoveId = appointmentToRemoveId;
			AggregateId = aggregateId;
			AggregateVersion = aggregateVersion;
		}

		public Guid                AppointmentToRemoveId { get; }
		public AggregateIdentifier AggregateId           { get; }
		public uint                AggregateVersion      { get; }
	}
}
