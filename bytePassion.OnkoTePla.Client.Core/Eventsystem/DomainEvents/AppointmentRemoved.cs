using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentRemoved : DomainEvent
	{
		private readonly Appointment removedAppointment;

		public AppointmentRemoved(AggregateIdentifier aggregateID, uint aggregateVersion, 
								  Guid userId, Tuple<Date, Time> timeStamp, 
								  Appointment removedAppointment)
			: base(aggregateID, aggregateVersion, userId, timeStamp)
		{
			this.removedAppointment = removedAppointment;
		}

		public Appointment RemovedAppointment
		{ 
			get { return removedAppointment; }
		}
	}
}
