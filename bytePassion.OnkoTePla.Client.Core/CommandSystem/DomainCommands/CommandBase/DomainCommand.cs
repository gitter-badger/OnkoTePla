using System;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands.CommandBase
{
	public class DomainCommand
	{
		private readonly AggregateIdentifier aggregateId;
		private readonly Guid userId;
		private readonly int aggregateVersion;

		public DomainCommand(AggregateIdentifier aggregateId, int aggregateVersion, Guid userId)
		{
			this.aggregateId = aggregateId;
			this.aggregateVersion = aggregateVersion;
			this.userId = userId;
		}
		
		public Guid UserId
		{
			get { return userId; }
		}		

		public AggregateIdentifier AggregateId
		{
			get { return aggregateId; }
		}

		public int AggregateVersion
		{
			get { return aggregateVersion; }
		}
	}
}
