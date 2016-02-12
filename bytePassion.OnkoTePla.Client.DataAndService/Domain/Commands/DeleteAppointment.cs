using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands
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
