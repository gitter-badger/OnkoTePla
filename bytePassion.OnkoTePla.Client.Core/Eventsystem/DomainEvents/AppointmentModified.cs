using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentModified : DomainEvent
	{
		public AppointmentModified(Guid aggregateID, int versionOfAggregate, Guid commandID) 
			: base(aggregateID, versionOfAggregate, commandID) {}
	}
}
