using System;
using bytePassion.Lib.TimeLib;

namespace bytePassion.OnkoTePla.Contracts.Domain.Events.Base
{
	public class DomainEvent
	{
		public DomainEvent (AggregateIdentifier aggregateId, 
							uint aggregateVersion, 
							Guid userId, 
							Guid patientId, 
							Tuple<Date, Time> timeStamp,
							ActionTag actionTag)
		{
			AggregateId = aggregateId;
			AggregateVersion = aggregateVersion;
			UserId = userId;
			TimeStamp = timeStamp;
			PatientId = patientId;
			ActionTag = actionTag;
		}
		
		public AggregateIdentifier   AggregateId      { get; }
		public uint                  AggregateVersion { get; }
		public Guid                  UserId           { get; }
		public Guid                  PatientId        { get; }
		public Tuple<Date, Time>     TimeStamp        { get; }
		public ActionTag             ActionTag        { get; }
	}
}
