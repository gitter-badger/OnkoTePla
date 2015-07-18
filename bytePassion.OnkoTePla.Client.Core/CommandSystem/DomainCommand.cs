using System;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{
	public class DomainCommand
	{
		private readonly AggregateIdentifier aggregateId;
		private readonly Guid userId;
		private readonly uint aggregateVersion;

		public DomainCommand(AggregateIdentifier aggregateId, uint aggregateVersion, Guid userId)
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

		public uint AggregateVersion
		{
			get { return aggregateVersion; }
		}
	}
}
