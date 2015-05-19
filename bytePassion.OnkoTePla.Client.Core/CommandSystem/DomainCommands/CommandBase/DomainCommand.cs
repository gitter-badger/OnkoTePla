using System;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands.CommandBase
{
	public class DomainCommand
	{
		private readonly Guid aggregateId;
		private readonly int aggregateVersion;

		public DomainCommand(Guid aggregateId, int aggregateVersion)
		{
			this.aggregateId = aggregateId;
			this.aggregateVersion = aggregateVersion;
		}

		public Guid AggregateId { get { return aggregateId; } }
		public int AggregateVersion { get { return aggregateVersion; } }
	}
}
