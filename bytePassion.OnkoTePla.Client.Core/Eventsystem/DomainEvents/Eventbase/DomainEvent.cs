using System;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase
{
	public class DomainEvent
	{
		private readonly Guid aggregateId;
		private readonly uint aggregateVersion;
		private readonly Guid eventId;
		private readonly Guid userId;
		private readonly Tuple<Date, Time> timeStamp; 

		public DomainEvent(Guid aggregateId, uint aggregateVersion, Guid eventId, Guid userId, Tuple<Date, Time> timeStamp)
		{
			this.aggregateId = aggregateId;
			this.aggregateVersion = aggregateVersion;
			this.eventId = eventId;
			this.userId = userId;
			this.timeStamp = timeStamp;
		}

		public Guid              EventId          { get { return eventId;          }}
		public Guid              AggregateId      { get { return aggregateId;      }}
		public uint              AggregateVersion { get { return aggregateVersion; }}
		public Guid              UserId           { get { return userId;           }}
		public Tuple<Date, Time> TimeStamp        { get { return timeStamp;        }} 
	}
}
