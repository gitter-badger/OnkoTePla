using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Eventsystem;
using System;


namespace bytePassion.OnkoTePla.Core.Domain.Commands
{
    public class ReplaceAppointment : DomainCommand
	{
		public ReplaceAppointment (AggregateIdentifier sourceAggregateId, 
								   AggregateIdentifier destinationAggregateId,
								   uint sourceAggregateVersion, 
								   uint destinationAggregateVersion,
								   Guid userId, Guid patientId, ActionTag actionTag,
								   string newDescription, Date newDate, 
								   Time newStartTime, Time newEndTime, 
								   Guid newTherapyPlaceId, 
								   Guid originalAppointmendId, Date originalDate)
			: base(userId, patientId, actionTag)
		{
			SourceAggregateId = sourceAggregateId;
			DestinationAggregateId = destinationAggregateId;
			SourceAggregateVersion = sourceAggregateVersion;
			DestinationAggregateVersion = destinationAggregateVersion;
			NewDescription = newDescription;
			NewDate = newDate;
			NewStartTime = newStartTime;
			NewEndTime = newEndTime;
			NewTherapyPlaceId = newTherapyPlaceId;
			OriginalAppointmendId = originalAppointmendId;
			OriginalDate = originalDate;
		}

		public AggregateIdentifier SourceAggregateId      { get; }
		public AggregateIdentifier DestinationAggregateId { get; }

		public uint SourceAggregateVersion      { get; }
		public uint DestinationAggregateVersion { get; }

		public string NewDescription        { get; }
		public Date   NewDate               { get; }
		public Time   NewStartTime          { get; }
		public Time   NewEndTime            { get; }
		public Guid   NewTherapyPlaceId     { get; }
		public Guid   OriginalAppointmendId { get; }
		public Date   OriginalDate          { get; }
		
	}
}
