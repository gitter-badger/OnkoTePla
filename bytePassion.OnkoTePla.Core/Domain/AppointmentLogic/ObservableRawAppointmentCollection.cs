using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Readmodels;

namespace bytePassion.OnkoTePla.Core.Domain.AppointmentLogic
{
	public class ObservableRawAppointmentCollection : INotifyRawAppointmentChanged,
													  INotifyCollectionChanged
	{
		public event EventHandler<RawAppointmentChangedEventArgs> AppointmentChanged;
		public event NotifyCollectionChangedEventHandler          CollectionChanged;

		private readonly IList<AppointmentTransferData> appointments;

		public ObservableRawAppointmentCollection ()
			: this(new List<AppointmentTransferData>())
		{
		}

		public ObservableRawAppointmentCollection (IEnumerable<AppointmentTransferData> initialAppointmentList)
		{
			appointments = initialAppointmentList.ToList();
		}

		public void AddAppointment (AppointmentTransferData newAppointment)
		{
			appointments.Add(newAppointment);

			AppointmentChanged?.Invoke(this, new RawAppointmentChangedEventArgs(newAppointment, ChangeAction.Added));
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
		}

		public void DeleteAppointment (Guid removedAppointmentId)
		{
			var appointmentToRemove = GetAppointmentById(removedAppointmentId);
			appointments.Remove(appointmentToRemove);

			AppointmentChanged?.Invoke(this, new RawAppointmentChangedEventArgs(appointmentToRemove, ChangeAction.Deleted));
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
		}

		public void ReplaceAppointment (AppointmentTransferData updatedAppointment)
		{
			var appointmentToRemove = GetAppointmentById(updatedAppointment.Id);
			appointments.Remove(appointmentToRemove);

			appointments.Add(updatedAppointment);

			AppointmentChanged?.Invoke(this, new RawAppointmentChangedEventArgs(updatedAppointment, ChangeAction.Modified));
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
																				 new List<AppointmentTransferData> { appointmentToRemove },
																				 new List<AppointmentTransferData> { updatedAppointment }));
		}

		public IEnumerable<AppointmentTransferData> Appointments
		{
			get { return appointments.ToList(); }
		}

		public AppointmentTransferData GetAppointmentById (Guid appointmentId)
		{
			return appointments.FirstOrDefault(appointment => appointment.Id == appointmentId);
		}
	}
}