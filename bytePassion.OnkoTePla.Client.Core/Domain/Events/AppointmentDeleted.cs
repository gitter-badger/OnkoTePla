using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Events
{
	public class AppointmentDeleted : DomainEvent
	{
		public AppointmentDeleted(AggregateIdentifier aggregateID, uint aggregateVersion, 
								  Guid userId, Guid patientId, Tuple<Date, Time> timeStamp,
								  Guid removedAppointmentId)
			: base(aggregateID, aggregateVersion, userId, patientId, timeStamp)
		{
			RemovedAppointmentId = removedAppointmentId;
		}

		public Guid RemovedAppointmentId { get; }
	}
}
