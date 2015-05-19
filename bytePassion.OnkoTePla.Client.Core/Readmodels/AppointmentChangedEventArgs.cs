using System;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class AppointmentChangedEventArgs : EventArgs
	{
		private readonly Appointment appointment;
		private readonly ChangeAction changeAction;

		public AppointmentChangedEventArgs(Appointment appointment, ChangeAction changeAction)
		{
			this.appointment = appointment;
			this.changeAction = changeAction;
		}

		public Appointment  Appointment  { get { return appointment;  }}
		public ChangeAction ChangeAction { get { return changeAction; }}
	}
}