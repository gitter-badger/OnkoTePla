using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentRemoved : DomainEvent
	{
		private readonly Appointment removedAppointment;

		public AppointmentRemoved(Guid aggregateID, int versionOfAggregate, Guid commandID, Appointment removedAppointment)
			: base(aggregateID, versionOfAggregate, commandID)
		{
			this.removedAppointment = removedAppointment;
		}

		public Appointment RemovedAppointment
		{ 
			get { return removedAppointment; }
		}
	}
}
