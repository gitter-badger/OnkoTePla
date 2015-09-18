using System;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Commands
{
	public class ReplaceAppointment : DomainCommand
	{
		public ReplaceAppointment (AggregateIdentifier aggregateId, uint aggregateVersion, 
								   Guid userId, Guid patientId, 
								   ReplaceAppointmentData replaceAppointmentData)
			: base(aggregateId, aggregateVersion, userId, patientId)
		{
			ReplaceAppointmentData = replaceAppointmentData;
		}

		public ReplaceAppointmentData ReplaceAppointmentData { get; }
	}
}
