using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Notification;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.AppointmentLogic
{
	public interface INotifyRawAppointmentChanged
	{
		event EventHandler<RawAppointmentChangedEventArgs> AppointmentChanged;
	}
}