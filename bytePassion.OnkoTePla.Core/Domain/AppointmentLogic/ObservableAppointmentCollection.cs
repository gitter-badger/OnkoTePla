using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Readmodels;


namespace bytePassion.OnkoTePla.Core.Domain.AppointmentLogic
{
	public class ObservableAppointmentCollection : INotifyAppointmentChanged, 
												   INotifyCollectionChanged
	{
		public event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;
		public event NotifyCollectionChangedEventHandler       CollectionChanged;

		private readonly IList<Appointment> appointments;

		public ObservableAppointmentCollection()
			: this(new List<Appointment>())
		{			
		}

	    public ObservableAppointmentCollection(IEnumerable<Appointment> initialAppointmentList)
	    {
		    appointments = initialAppointmentList.ToList();
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

		public void ReplaceAppointment(Appointment updatedAppointment)
		{
			var appointmentToRemove = GetAppointmentById(updatedAppointment.Id);
			appointments.Remove(appointmentToRemove);

			appointments.Add(updatedAppointment);

			AppointmentChanged?.Invoke(this, new AppointmentChangedEventArgs(updatedAppointment, ChangeAction.Modified));
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, 
																				 new List<Appointment> {appointmentToRemove}, 
																				 new List<Appointment> {updatedAppointment}));
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
