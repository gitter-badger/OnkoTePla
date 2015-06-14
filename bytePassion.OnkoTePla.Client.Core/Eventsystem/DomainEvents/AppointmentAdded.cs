using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentAdded : DomainEvent
	{
		private readonly Guid   patientId;
		private readonly string description;		
		private readonly Time   startTime;
		private readonly Time   endTime;
		private readonly Guid   therapyPlaceId;
		private readonly Guid	roomId;

		public AppointmentAdded(AggregateIdentifier aggregateID, uint aggregateVersion, 
								Guid userId, Tuple<Date, Time> timeStamp,
							    Guid patientId, string description, 
								Time startTime, Time endTime, 
								Guid therapyPlaceId, Guid roomId)
			: base(aggregateID, aggregateVersion, userId, timeStamp)
		{
			this.patientId      = patientId;
			this.description    = description;			
			this.startTime      = startTime;
			this.endTime        = endTime;
			this.therapyPlaceId = therapyPlaceId;
			this.roomId         = roomId;			
		}

		public Guid   PatientId      { get { return patientId;        }}
		public string Description    { get { return description;      }}
		public Date   Day            { get { return AggregateId.Date; }}
		public Time   StartTime      { get { return startTime;        }}
		public Time   EndTime        { get { return endTime;          }}
		public Guid   TherapyPlaceId { get { return therapyPlaceId;   }}
		public Guid   RoomId         { get { return roomId;           }}
	}
}
