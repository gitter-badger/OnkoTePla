using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands
{
	public class ReplaceAppointment : DomainCommand
	{
		public ReplaceAppointment (AggregateIdentifier sourceAggregateId, 
								   AggregateIdentifier destinationAggregateId,
								   uint sourceAggregateVersion, 
								   uint destinationAggregateVersion,
								   Guid userId, Guid patientId, ActionTag actionTag,

								   string originalDescription, string newDescription,
								   Date   originalDate,          Date newDate,
								   Time   originalStartTime,     Time newStartTime,
								   Time   originalEndTime,       Time newEndTime,
								   Guid   originalTherapyPlaceId,Guid newTherapyPlaceId, 
								   Guid   originalAppointmendId)

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
			OriginalDescription = originalDescription;
			OriginalStartTime = originalStartTime;
			OriginalEndTime = originalEndTime;
			OriginalTherapyPlaceId = originalTherapyPlaceId;
		}

		public AggregateIdentifier SourceAggregateId      { get; }
		public AggregateIdentifier DestinationAggregateId { get; }

		public uint SourceAggregateVersion      { get; }
		public uint DestinationAggregateVersion { get; }

		public string OriginalDescription    { get; }
		public Date   OriginalDate           { get; }
		public Time   OriginalStartTime      { get; }
		public Time   OriginalEndTime        { get; }
		public Guid   OriginalTherapyPlaceId { get; }
		public Guid   OriginalAppointmendId  { get; }

		public string NewDescription         { get; }		
		public Date   NewDate                { get; }		
		public Time   NewStartTime           { get; }		
		public Time   NewEndTime             { get; }		
		public Guid   NewTherapyPlaceId      { get; }
						
	}
}
