
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public class AppointmentsOfDayAggregate : AggregateRootBase<EventStreamIdentifier>
	{

		private IList<Appointment> appointments;

		public AppointmentsOfDayAggregate(EventStreamIdentifier id, uint version) 
			: base(id, version)
		{
			appointments = new List<Appointment>();
		}		

		protected void Apply (AppointmentAdded @event)
		{
			appointments.Add(new Appointment(@event.Patient, @event.TherapyPlace, @event.Room, 
											 @event.Day, @event.StartTime, @event.EndTime));
		}		
	}
}
