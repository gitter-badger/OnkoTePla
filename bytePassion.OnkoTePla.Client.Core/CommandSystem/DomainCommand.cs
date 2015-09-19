using System;

namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{
	public class DomainCommand
	{
		public DomainCommand(Guid userId, Guid patientId)
		{			
			UserId = userId;
			PatientId = patientId;
		}
		
		public Guid UserId    { get; }
		public Guid PatientId { get; }		
	}
}
