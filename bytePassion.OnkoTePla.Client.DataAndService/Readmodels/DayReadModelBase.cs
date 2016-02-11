using System;
using bytePassion.OnkoTePla.Client.DataAndService.EventBus;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Core.Readmodels;

namespace bytePassion.OnkoTePla.Client.DataAndService.Readmodels
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