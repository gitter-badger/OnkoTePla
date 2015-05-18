using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentAdded : DomainEvent
	{
		public AppointmentAdded(Guid aggregateID, int versionOfAggregate, Guid commandID) 
			: base(aggregateID, versionOfAggregate, commandID) {}
	}
}
