using System;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Core.Readmodels
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