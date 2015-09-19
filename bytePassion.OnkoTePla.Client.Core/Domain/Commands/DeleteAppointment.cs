using System;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Commands
{
	public class DeleteAppointment : DomainCommand
	{
		public DeleteAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, 
								 Guid userId, Guid appointmentId, Guid patientId)
			: base(userId, patientId)
		{
			AppointmentId = appointmentId;
			AggregateId = aggregateId;
			AggregateVersion = aggregateVersion;
		}

		public Guid                AppointmentId    { get; }
		public AggregateIdentifier AggregateId      { get; }
		public uint                AggregateVersion { get; }
	}
}
