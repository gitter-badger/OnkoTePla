using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Commands
{
	public class AddAppointment : DomainCommand
	{
		public AddAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, Guid userId, 
							  Guid patientId, string description, 
							  Time startTime, Time endTime, 
							  Guid therapyPlaceId)
			: base(aggregateId, aggregateVersion, userId, patientId)
		{
			CreateAppointmentData = new CreateAppointmentData(patientId, description, 
															  startTime, endTime, AggregateId.Date, 
															  therapyPlaceId, Guid.NewGuid());
		}

		public CreateAppointmentData CreateAppointmentData { get; }
	}
}
