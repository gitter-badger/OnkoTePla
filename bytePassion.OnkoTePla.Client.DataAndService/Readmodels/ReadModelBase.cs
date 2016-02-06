using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Readmodels;
using IClientEventBus = bytePassion.OnkoTePla.Client.DataAndService.EventBus.IClientEventBus;


namespace bytePassion.OnkoTePla.Client.DataAndService.Readmodels
{
	public abstract class ReadModelBase : DisposingObject, INotifyAppointmentChanged
														   
	{				
		public abstract event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;

		private readonly IClientEventBus eventBus;

		protected ReadModelBase (IClientEventBus eventBus)
		{
			this.eventBus = eventBus;			

			RegisterAtEventBus();
		}
		
		public abstract void Process(AppointmentAdded    domainEvent, Action<string> errorCallback); 
		public abstract void Process(AppointmentReplaced domainEvent);
		public abstract void Process(AppointmentDeleted  domainEvent);
				
		private void RegisterAtEventBus ()
		{
			eventBus.RegisterReadModel(this);			
		}

		private void DeregisterAtEventBus ()
		{
			eventBus.DeregisterReadModel(this);			
		}

        protected override void CleanUp()
		{
			DeregisterAtEventBus();
		}
	}
}
