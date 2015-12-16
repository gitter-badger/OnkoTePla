using bytePassion.OnkoTePla.Contracts.Appointments;
using System;


namespace bytePassion.OnkoTePla.Core.Readmodels
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