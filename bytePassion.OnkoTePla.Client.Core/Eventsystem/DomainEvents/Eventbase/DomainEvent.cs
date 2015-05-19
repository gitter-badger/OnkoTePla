
using System;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase
{
	public class DomainEvent
	{
		private readonly Guid aggregateId;
		private readonly int  aggregateVersion;
		private readonly Guid eventId;

		public DomainEvent(Guid aggregateID, int aggregateVersion, Guid eventId)
		{
			this.aggregateId = aggregateID;
			this.aggregateVersion = aggregateVersion;
			this.eventId = eventId;
		}

		public Guid EventId          { get { return eventId;          }}
		public Guid AggregateId      { get { return aggregateId;      }}
		public int  AggregateVersion { get { return aggregateVersion; }}
	}
}
