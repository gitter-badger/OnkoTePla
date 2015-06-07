using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentModified : DomainEvent
	{
		public AppointmentModified (AggregateIdentifier aggregateID, uint aggregateVersion,Guid userId, Tuple<Date, Time> timeStamp)
			: base(aggregateID, aggregateVersion, userId, timeStamp)
		{
			
		}
	}
}
