using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Core.Eventsystem;
using System;


namespace bytePassion.OnkoTePla.Core.Domain.Events
{
    public class AppointmentDeleted : DomainEvent
	{
		public AppointmentDeleted(AggregateIdentifier aggregateID, uint aggregateVersion, 
								  Guid userId, Guid patientId, Tuple<Date, Time> timeStamp, 
								  ActionTag actionTag, Guid removedAppointmentId)
			: base(aggregateID, aggregateVersion, userId, patientId, timeStamp, actionTag)
		{
			RemovedAppointmentId = removedAppointmentId;
		}
		
		public Guid RemovedAppointmentId { get; }
	}
}
