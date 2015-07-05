using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Events
{
	public class AppointmentDeleted : DomainEvent
	{
		private readonly Guid removedAppointmentId;

		public AppointmentDeleted(AggregateIdentifier aggregateID, uint aggregateVersion, 
								  Guid userId, Guid patientId, Tuple<Date, Time> timeStamp,
								  Guid removedAppointmentId)
			: base(aggregateID, aggregateVersion, userId, patientId, timeStamp)
		{
			this.removedAppointmentId = removedAppointmentId;
		}

		public Guid RemovedAppointmentId
		{
			get { return removedAppointmentId; }
		}
	}
}
