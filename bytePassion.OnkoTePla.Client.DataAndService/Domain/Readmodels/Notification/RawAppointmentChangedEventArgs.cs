using System;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Notification
{
	public class RawAppointmentChangedEventArgs : EventArgs
	{
		public RawAppointmentChangedEventArgs (AppointmentTransferData appointment, ChangeAction changeAction)
		{
			Appointment = appointment;
			ChangeAction = changeAction;
		}

		public AppointmentTransferData Appointment { get; }
		public ChangeAction           ChangeAction { get; }
	}
}