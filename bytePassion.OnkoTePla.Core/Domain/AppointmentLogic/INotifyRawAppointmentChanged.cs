using System;
using bytePassion.OnkoTePla.Core.Readmodels;

namespace bytePassion.OnkoTePla.Core.Domain.AppointmentLogic
{
	public interface INotifyRawAppointmentChanged
	{
		event EventHandler<RawAppointmentChangedEventArgs> AppointmentChanged;
	}
}