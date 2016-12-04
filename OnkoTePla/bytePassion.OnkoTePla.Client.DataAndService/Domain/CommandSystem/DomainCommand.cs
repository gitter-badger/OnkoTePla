using System;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem
{
	public class DomainCommand
	{
		public DomainCommand(Guid userId, Guid patientId, ActionTag actionTag)
		{			
			UserId = userId;
			PatientId = patientId;
			ActionTag = actionTag;
		}
		
		public Guid      UserId    { get; }
		public Guid      PatientId { get; }	
		public ActionTag ActionTag { get; }	
	}
}
