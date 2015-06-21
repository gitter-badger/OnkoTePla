using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Events
{
	public class AppointmentRemoved : DomainEvent
	{		
		public AppointmentRemoved(AggregateIdentifier aggregateID, uint aggregateVersion, 
								  Guid userId, Guid patientId, Tuple<Date, Time> timeStamp)
			: base(aggregateID, aggregateVersion, userId, patientId, timeStamp)
		{
			
		}		
	}
}
