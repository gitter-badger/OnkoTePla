
using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public class AppointmentsOfDayAggregate : AggregateRootBase
	{

		private IList<Appointment> appointments;
		private readonly uint configVersion;

		public AppointmentsOfDayAggregate(Guid id, uint version, uint configVersion) 
			: base(id, version)
		{
			this.configVersion = configVersion;
			appointments = new List<Appointment>();
		}

		protected void Apply (AppointmentAdded @event)
		{
			appointments.Add(new Appointment(@event.Patient, @event.TherapyPlace, @event.Room, 
											 @event.Day, @event.StartTime, @event.EndTime));
		}
	}
}
