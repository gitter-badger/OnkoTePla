using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using System;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public abstract class ReadModelBase : DisposingObject, INotifyAppointmentChanged,
														   IDomainEventHandler<AppointmentAdded>,
													       IDomainEventHandler<AppointmentReplaced>,
													       IDomainEventHandler<AppointmentDeleted>
	{				
		public abstract event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;

		private readonly IEventBus eventBus;

		protected ReadModelBase (IEventBus eventBus)
		{
			this.eventBus = eventBus;			

			RegisterAtEventBus();
		}
		
		public abstract void Process(AppointmentAdded    domainEvent);
		public abstract void Process(AppointmentReplaced domainEvent);
		public abstract void Process(AppointmentDeleted  domainEvent);
				
		private void RegisterAtEventBus ()
		{
			eventBus.RegisterEventHandler<AppointmentAdded>(this);
			eventBus.RegisterEventHandler<AppointmentReplaced>(this);
			eventBus.RegisterEventHandler<AppointmentDeleted>(this);
		}

		private void DeregisterAtEventBus ()
		{
			eventBus.DeregisterEventHander<AppointmentAdded>(this);
			eventBus.DeregisterEventHander<AppointmentReplaced>(this);
			eventBus.DeregisterEventHander<AppointmentDeleted>(this);
		}

        protected override void CleanUp()
		{
			DeregisterAtEventBus();
		}
	}
}
