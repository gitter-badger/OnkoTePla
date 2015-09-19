using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Events
{
	public class AppointmentReplaced : DomainEvent
	{
		public AppointmentReplaced (AggregateIdentifier aggregateId, uint aggregateVersion,
								    Guid userId, Guid patientId, Tuple<Date, Time> timeStamp,
								    ReplaceAppointmentData replaceAppointmentData)
			: base(aggregateId, aggregateVersion, userId, patientId, timeStamp)
		{
			ReplaceAppointmentData = replaceAppointmentData;			
		}

		public ReplaceAppointmentData ReplaceAppointmentData { get; }
	}
}
