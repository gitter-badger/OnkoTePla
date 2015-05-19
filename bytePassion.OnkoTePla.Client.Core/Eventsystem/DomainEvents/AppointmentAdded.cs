using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentAdded : DomainEvent
	{
		private readonly Appointment addedAppointment;

		public AppointmentAdded(Guid aggregateID, int versionOfAggregate, Guid commandID, Appointment addedAppointment)
			: base(aggregateID, versionOfAggregate, commandID)
		{
			this.addedAppointment = addedAppointment;
		}

		public Appointment AddedAppointment
		{
			get { return addedAppointment; }
		}
	}
}
