using bytePassion.OnkoTePla.Core.Readmodels;
using System;


namespace bytePassion.OnkoTePla.Core.Domain.AppointmentLogic
{
    public interface INotifyAppointmentChanged
	{
		event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;
	}
}