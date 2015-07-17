using System;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Commands
{
	public class DeleteAppointment : DomainCommand
	{
		private readonly Guid appointmentId;
		private readonly Guid patientId;

		public DeleteAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, 
								 Guid userId, Guid appointmentId, Guid patientId)
			: base(aggregateId, aggregateVersion, userId)
		{
			this.appointmentId = appointmentId;
			this.patientId = patientId;
		}

		public Guid AppointmentId
		{
			get { return appointmentId; }
		}

		public Guid PatientId
		{
			get { return patientId; }
		}
	}
}
