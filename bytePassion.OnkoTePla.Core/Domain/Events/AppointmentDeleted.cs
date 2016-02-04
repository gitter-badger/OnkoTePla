using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Core.Domain.Events
{
	public class AppointmentDeleted : DomainEvent
	{
		public AppointmentDeleted(AggregateIdentifier aggregateId, uint aggregateVersion, 
								  Guid userId, Guid patientId, Tuple<Date, Time> timeStamp, 
								  ActionTag actionTag, Guid removedAppointmentId)
			: base(aggregateId, aggregateVersion, userId, patientId, timeStamp, actionTag)
		{
			RemovedAppointmentId = removedAppointmentId;
		}
		
		public Guid RemovedAppointmentId { get; }
	}
}
