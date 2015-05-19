using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentRemoved : DomainEvent
	{
		private readonly Appointment removedAppointment;

		public AppointmentRemoved(Guid aggregateID, int aggregateVersion, Guid eventID, Appointment removedAppointment)
			: base(aggregateID, aggregateVersion, eventID)
		{
			this.removedAppointment = removedAppointment;
		}

		public Appointment RemovedAppointment
		{ 
			get { return removedAppointment; }
		}
	}
}
