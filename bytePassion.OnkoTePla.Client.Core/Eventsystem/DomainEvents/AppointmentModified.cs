using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentModified : DomainEvent
	{
		public AppointmentModified (Guid aggregateID, uint aggregateVersion, Guid eventID, Guid userId, Tuple<Date, Time> timeStamp)
			: base(aggregateID, aggregateVersion, eventID, userId, timeStamp)
		{
			
		}
	}
}
