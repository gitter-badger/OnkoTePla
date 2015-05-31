using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentModified : DomainEvent
	{
		public AppointmentModified(Guid aggregateID, uint aggregateVersion, Guid eventID)
			: base(aggregateID, aggregateVersion, eventID)
		{
			
		}
	}
}
