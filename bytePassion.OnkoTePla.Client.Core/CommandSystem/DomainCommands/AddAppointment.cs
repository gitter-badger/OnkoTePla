using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands.CommandBase;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands
{
	public class AddAppointment : DomainCommand
	{
		private readonly Guid   patientId;
		private readonly string description;		
		private readonly Time   startTime;
		private readonly Time   endTime;
		private readonly Guid   therapyPlaceId;
		private readonly Guid   roomId;

		public AddAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, Guid userId, 
							  Guid patientId, string description, 
							  Time startTime, Time endTime, 
							  Guid therapyPlaceId, Guid roomId)
			: base(aggregateId, aggregateVersion, userId)
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
		public Guid   Room           { get { return roomId;           }}
	}
}
