using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Events
{
	public class AppointmentReplaced : DomainEvent
	{
		public AppointmentReplaced (AggregateIdentifier aggregateID, uint aggregateVersion,Guid userId, Guid patientId, Tuple<Date, Time> timeStamp)
			: base(aggregateID, aggregateVersion, userId, patientId, timeStamp)
		{
			
		}
	}
}
