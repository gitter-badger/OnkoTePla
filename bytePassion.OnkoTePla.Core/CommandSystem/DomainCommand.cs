using bytePassion.OnkoTePla.Core.Eventsystem;
using System;


namespace bytePassion.OnkoTePla.Core.CommandSystem
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
