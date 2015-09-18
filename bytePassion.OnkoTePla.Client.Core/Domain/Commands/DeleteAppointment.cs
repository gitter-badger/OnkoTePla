using System;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Commands
{
	public class DeleteAppointment : DomainCommand
	{
		public DeleteAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, 
								 Guid userId, Guid appointmentId, Guid patientId)
			: base(aggregateId, aggregateVersion, userId, patientId)
		{
			AppointmentId = appointmentId;
		}

		public Guid AppointmentId { get; }
	}
}
