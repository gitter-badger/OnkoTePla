using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Commands
{
	public class AddAppointment : DomainCommand
	{
		public AddAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, 
							  Guid userId, ActionTag actionTag,
							  Guid patientId, string description, 
							  Time startTime, Time endTime, 
							  Guid therapyPlaceId)
			: base(userId, patientId, actionTag)
		{
			CreateAppointmentData = new CreateAppointmentData(patientId, description, 
															  startTime, endTime, AggregateId.Date, 
															  therapyPlaceId, Guid.NewGuid());
			AggregateId = aggregateId;
			AggregateVersion = aggregateVersion;
		}

		public CreateAppointmentData CreateAppointmentData { get; }
		public AggregateIdentifier   AggregateId           { get; }
		public uint                  AggregateVersion      { get; }
	}
}
