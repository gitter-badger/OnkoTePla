using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic
{
	public class ObservableAppointmentCollection : INotifyAppointmentChanged, 
												   INotifyCollectionChanged
	{
		public event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;
		public event NotifyCollectionChangedEventHandler       CollectionChanged;

		private readonly IList<Appointment> appointments;

		public ObservableAppointmentCollection()
		{
			appointments = new List<Appointment>();
		}

		public void AddAppointment(Appointment newAppointment)
		{
			appointments.Add(newAppointment);
			
			AppointmentChanged?.Invoke(this, new AppointmentChangedEventArgs(newAppointment, ChangeAction.Added));			
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
		}

		public void DeleteAppointment (Guid removedAppointmentId)
		{
			var appointmentToRemove = GetAppointmentById(removedAppointmentId);
			appointments.Remove(appointmentToRemove);

			AppointmentChanged?.Invoke(this, new AppointmentChangedEventArgs(appointmentToRemove, ChangeAction.Deleted));
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
		}

		public IEnumerable<Appointment> Appointments
		{
			get { return appointments.ToList(); }
		}

		public Appointment GetAppointmentById(Guid appointmentId)
		{
			return appointments.FirstOrDefault(appointment => appointment.Id == appointmentId);
		}		
	}
}
