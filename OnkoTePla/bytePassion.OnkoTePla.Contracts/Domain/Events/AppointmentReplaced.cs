using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Contracts.Domain.Events
{
	public class AppointmentReplaced : DomainEvent
	{
		public AppointmentReplaced (AggregateIdentifier aggregateId, uint aggregateVersion,
								    Guid userId, Guid patientId, Tuple<Date, Time> timeStamp,
									ActionTag actionTag,
									string newDescription, Date newDate,
								    Time newStartTime, Time newEndTime,
								    Guid newTherapyPlaceId, Guid newLabelId,
									Guid originalAppointmendId)
			: base(aggregateId, aggregateVersion, userId, patientId, timeStamp, actionTag)
		{
			NewDescription = newDescription;
			NewDate = newDate;
			NewStartTime = newStartTime;
			NewEndTime = newEndTime;
			NewTherapyPlaceId = newTherapyPlaceId;
			OriginalAppointmendId = originalAppointmendId;
			NewLabelId = newLabelId;
		}

		public string NewDescription        { get; }
		public Date   NewDate               { get; }
		public Time   NewStartTime          { get; }
		public Time   NewEndTime            { get; }
		public Guid   NewTherapyPlaceId     { get; }
		public Guid   NewLabelId            { get; }
		public Guid   OriginalAppointmendId { get; }
	}
}
