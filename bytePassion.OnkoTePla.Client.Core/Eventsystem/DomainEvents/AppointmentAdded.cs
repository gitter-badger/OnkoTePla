using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentAdded : DomainEvent
	{
		private readonly Appointment addedAppointment;

		public AppointmentAdded(Guid aggregateID, int aggregateVersion, Guid eventID, Appointment addedAppointment)
			: base(aggregateID, aggregateVersion, eventID)
		{
			this.addedAppointment = addedAppointment;
		}

		public Appointment AddedAppointment
		{
			get { return addedAppointment; }
		}
	}
}
