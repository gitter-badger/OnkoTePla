
using System;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase
{
	public class DomainEvent
	{
		private readonly Guid aggregateID;
		private readonly int  versionOfAggregate;
		private readonly Guid commandID;

		public DomainEvent(Guid aggregateID, int versionOfAggregate, Guid commandID)
		{
			this.aggregateID = aggregateID;
			this.versionOfAggregate = versionOfAggregate;
			this.commandID = commandID;
		}

		public Guid CommandID          { get { return commandID;          }}
		public Guid AggregateID        { get { return aggregateID;        }}
		public int  VersionOfAggregate { get { return versionOfAggregate; }}
	}
}
