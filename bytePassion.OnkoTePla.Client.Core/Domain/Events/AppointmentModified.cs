using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Events
{
	public class AppointmentModified : DomainEvent
	{
		public AppointmentModified (AggregateIdentifier aggregateID, uint aggregateVersion,Guid userId, Tuple<Date, Time> timeStamp)
			: base(aggregateID, aggregateVersion, userId, timeStamp)
		{
			
		}
	}
}
