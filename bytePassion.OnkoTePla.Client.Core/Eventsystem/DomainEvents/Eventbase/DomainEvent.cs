using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase
{
	public class DomainEvent
	{
		private readonly AggregateIdentifier aggregateId;
		private readonly uint aggregateVersion;
		private readonly Guid userId;
		private readonly Tuple<Date, Time> timeStamp; 

		public DomainEvent(AggregateIdentifier aggregateId, uint aggregateVersion, Guid userId, Tuple<Date, Time> timeStamp)
		{
			this.aggregateId = aggregateId;
			this.aggregateVersion = aggregateVersion;
			this.userId = userId;
			this.timeStamp = timeStamp;
		}
		
		public AggregateIdentifier AggregateId      { get { return aggregateId;      }}
		public uint                  AggregateVersion { get { return aggregateVersion; }}
		public Guid                  UserId           { get { return userId;           }}
		public Tuple<Date, Time>     TimeStamp        { get { return timeStamp;        }} 
	}
}
