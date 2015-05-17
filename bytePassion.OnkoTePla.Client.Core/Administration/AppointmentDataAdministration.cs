using System.Collections.Concurrent;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Administration
{
	
	public class AppointmentDataAdministration
	{
		private IDictionary<Date,        IList<Appointment>> currentlyLoadedAppointments;
		private IDictionary<Appointment, IList<DomainEvent>> changesToCurrentAppointments; 


		public AppointmentDataAdministration(IReadOnlyList<Appointment> initialAppointmentData) 
		{
			currentlyLoadedAppointments = new ConcurrentDictionary<Date, IList<Appointment>>();
			changesToCurrentAppointments = new ConcurrentDictionary<Appointment, IList<DomainEvent>>();

			if (initialAppointmentData == null) return;
			
			foreach (var appointment in initialAppointmentData)
			{
				var date = new Date(appointment.StartTime);

				if (!currentlyLoadedAppointments.ContainsKey(date))
					currentlyLoadedAppointments.Add(date, new List<Appointment>());

				currentlyLoadedAppointments[date].Add(appointment);
			}
		}

		public void AddAppointment(AppointmentAdded appointmentAddedEvent)
		{
			
		}
	}
}
