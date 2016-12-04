using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Notification;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Base
{
	public abstract class DayReadModelBase : ReadModelBase, INotifyAppointmentChanged
	{
		protected DayReadModelBase(IClientEventBus eventBus) 
			: base(eventBus)
		{
		}
		
		public abstract event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;
	}
}