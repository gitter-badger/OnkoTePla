using System;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands.CommandBase
{
	public class DomainCommand
	{
		private readonly EventStreamIdentifier id;
		private readonly Guid userId;
		private readonly int aggregateVersion;

		public DomainCommand(EventStreamIdentifier id, int aggregateVersion, Guid userId)
		{
			this.id = id;
			this.aggregateVersion = aggregateVersion;
			this.userId = userId;
		}
		
		public Guid UserId
		{
			get { return userId; }
		}		

		public EventStreamIdentifier Id
		{
			get { return id; }
		}

		public int AggregateVersion
		{
			get { return aggregateVersion; }
		}
	}
}
