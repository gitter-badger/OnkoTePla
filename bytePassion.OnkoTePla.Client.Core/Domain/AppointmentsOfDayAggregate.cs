
using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public class AppointmentsOfDayAggregate : AggregateRootBase<AggregateIdentifier>
	{

		private IList<Appointment> appointments;

		public AppointmentsOfDayAggregate(AggregateIdentifier id, uint version) 
			: base(id, version)
		{
			appointments = new List<Appointment>();
		}		

		protected void Apply (AppointmentAdded @event)
		{
			appointments.Add(new Appointment(@event.Patient, @event.TherapyPlace, @event.Room, 
											 @event.Day, @event.StartTime, @event.EndTime));
		}
		
		public void AddAppointment(Guid userId,
								   Patient patient, string description, 
								   Date day, Time startTime, Time endTime,
								   TherapyPlace therapyPlace, Room room) 
		{
 			ApplyChange(new AppointmentAdded(Id, Version, userId,TimeTools.GetCurrentTimeStamp(), 
											 patient, description, day, startTime, endTime, 
											 therapyPlace, room ));
		}
	}
}
