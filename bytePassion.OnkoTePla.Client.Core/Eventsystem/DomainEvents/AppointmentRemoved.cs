using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentRemoved : DomainEvent
	{
		public AppointmentRemoved(Guid aggregateID, int versionOfAggregate, Guid commandID)
			: base(aggregateID, versionOfAggregate, commandID)
		{
			
		}
	}
}
