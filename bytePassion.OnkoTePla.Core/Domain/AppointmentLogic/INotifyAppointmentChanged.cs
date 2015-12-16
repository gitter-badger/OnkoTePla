using System;
using bytePassion.OnkoTePla.Client.Core.Readmodels;


namespace bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic
{
	public interface INotifyAppointmentChanged
	{
		event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;
	}
}