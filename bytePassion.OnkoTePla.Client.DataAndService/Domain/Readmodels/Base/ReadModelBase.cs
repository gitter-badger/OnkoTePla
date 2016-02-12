using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus;
using bytePassion.OnkoTePla.Contracts.Domain.Events;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Base
{
	public abstract class ReadModelBase : DisposingObject, IDomainEventHandler<AppointmentAdded>,
													       IDomainEventHandler<AppointmentReplaced>,
													       IDomainEventHandler<AppointmentDeleted>
	{				
		private readonly IClientEventBus eventBus;

		protected ReadModelBase (IClientEventBus eventBus)
		{
			this.eventBus = eventBus;			

			RegisterAtEventBus();
		}
		
		public abstract void Process(AppointmentAdded    domainEvent);
		public abstract void Process(AppointmentReplaced domainEvent);
		public abstract void Process(AppointmentDeleted  domainEvent);
				
		private void RegisterAtEventBus ()
		{
			eventBus.RegisterReadModel(this);

			//eventBus.RegisterEventHandler<AppointmentAdded>(this);
			//eventBus.RegisterEventHandler<AppointmentReplaced>(this);
			//eventBus.RegisterEventHandler<AppointmentDeleted>(this);
		}

		private void DeregisterAtEventBus ()
		{
			eventBus.DeregisterReadModel(this);

			//eventBus.DeregisterEventHander<AppointmentAdded>(this);
			//eventBus.DeregisterEventHander<AppointmentReplaced>(this);
			//eventBus.DeregisterEventHander<AppointmentDeleted>(this);
		}

        protected override void CleanUp()
		{
			DeregisterAtEventBus();
		}
	}
}
