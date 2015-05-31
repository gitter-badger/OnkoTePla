
using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public class AppointmentsOfDayAggregate : AggregateRootBase
	{

		private IList<Appointment> appointments; 

		public AppointmentsOfDayAggregate(Guid id, uint version) 
			: base(id, version)
		{
			appointments = new List<Appointment>();
		}

		protected void Apply (AppointmentAdded addedAppointmentEvent)
		{
			appointments.Add(addedAppointmentEvent.AddedAppointment);
		}
	}
}
