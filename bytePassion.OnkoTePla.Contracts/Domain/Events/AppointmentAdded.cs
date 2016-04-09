using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Contracts.Domain.Events
{
	public class AppointmentAdded : DomainEvent
	{
		public AppointmentAdded(AggregateIdentifier aggregateId, uint aggregateVersion, 
								Guid userId, Tuple<Date, Time> timeStamp, ActionTag actionTag,
								Guid patientId, string description,
								Time startTime, Time endTime,
								Guid therapyPlaceId, Guid labelId, 
								Guid appointmentId)
			: base(aggregateId, aggregateVersion, userId, patientId, timeStamp, actionTag)
		{
			Description = description;
			StartTime = startTime;
			EndTime = endTime;
			TherapyPlaceId = therapyPlaceId;
			AppointmentId = appointmentId;
			LabelId = labelId;
		}

		public string Description    { get; }		
		public Time   StartTime      { get; }
		public Time   EndTime        { get; }
		public Guid   TherapyPlaceId { get; }
		public Guid	  LabelId        { get; }
		public Guid   AppointmentId  { get; }
	}
}
