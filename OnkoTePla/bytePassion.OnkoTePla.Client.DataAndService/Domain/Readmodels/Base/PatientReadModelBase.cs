using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Notification;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Base
{
	public abstract class PatientReadModelBase : ReadModelBase, INotifyRawAppointmentChanged
	{
		protected PatientReadModelBase(IClientEventBus eventBus) 
			: base(eventBus)
		{
		}

		public abstract event EventHandler<RawAppointmentChangedEventArgs> AppointmentChanged;
	}
}