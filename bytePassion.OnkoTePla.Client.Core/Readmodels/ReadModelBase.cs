using System;
using bytePassion.Lib.Messaging;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public abstract class ReadModelBase : IDisposable, INotifyAppointmentChanged,
													   IDomainEventHandler<AppointmentAdded>,
													   IDomainEventHandler<AppointmentReplaced>,
													   IDomainEventHandler<AppointmentDeleted>
	{

		public abstract event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;

		private readonly IMessageBus<DomainEvent> eventBus;

		protected ReadModelBase (IMessageBus<DomainEvent> eventBus)
		{
			this.eventBus = eventBus;

			RegisterAtEventBus();
		}
		
		public abstract void Process(AppointmentAdded    domainEvent);
		public abstract void Process(AppointmentReplaced domainEvent);
		public abstract void Process(AppointmentDeleted  domainEvent);
				
		private void RegisterAtEventBus ()
		{
			eventBus.RegisterMessageHandler<AppointmentAdded>(this);
			eventBus.RegisterMessageHandler<AppointmentReplaced>(this);
			eventBus.RegisterMessageHandler<AppointmentDeleted>(this);
		}

		private void DeregisterAtEventBus ()
		{
			eventBus.DeregisterMessageHander<AppointmentAdded>(this);
			eventBus.DeregisterMessageHander<AppointmentReplaced>(this);
			eventBus.DeregisterMessageHander<AppointmentDeleted>(this);
		}

		private bool disposed = false;
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ReadModelBase ()
		{
			Dispose(false);
		}

		private void Dispose (bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
					DeregisterAtEventBus();

			}
			disposed = true;
		}
	}
}
