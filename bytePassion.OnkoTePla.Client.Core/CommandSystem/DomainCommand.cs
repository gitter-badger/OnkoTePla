using System;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{
	public class DomainCommand
	{
		public DomainCommand(AggregateIdentifier aggregateId, uint aggregateVersion, 
							 Guid userId, Guid patientId)
		{
			AggregateId = aggregateId;
			AggregateVersion = aggregateVersion;
			UserId = userId;
			PatientId = patientId;
		}
		
		public Guid                UserId           { get; }
		public Guid                PatientId        { get; }
		public AggregateIdentifier AggregateId      { get; }
		public uint                AggregateVersion { get; }
	}
}
