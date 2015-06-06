using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentRemoved : DomainEvent
	{
		private readonly Appointment removedAppointment;

		public AppointmentRemoved(Guid aggregateID, uint aggregateVersion, Guid eventID, Guid userId, Tuple<Date, Time> timeStamp, Appointment removedAppointment)
			: base(aggregateID, aggregateVersion, eventID, userId, timeStamp)
		{
			this.removedAppointment = removedAppointment;
		}

		public Appointment RemovedAppointment
		{ 
			get { return removedAppointment; }
		}
	}
}
