using System;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Notification
{
	public class AppointmentChangedEventArgs : EventArgs
	{
		public AppointmentChangedEventArgs(Appointment appointment, ChangeAction changeAction)
		{
			Appointment = appointment;
			ChangeAction = changeAction;
		}

		public Appointment  Appointment  { get; }
		public ChangeAction ChangeAction { get; }
	}
}