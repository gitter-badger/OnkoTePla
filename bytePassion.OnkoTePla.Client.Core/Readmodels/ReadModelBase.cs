using System;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Bus;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public abstract class ReadModelBase : IDisposable, INotifyAppointmentChanged,
													   IDomainEventHandler<AppointmentAdded>,
													   IDomainEventHandler<AppointmentModified>,
													   IDomainEventHandler<AppointmentRemoved>
	{

		public abstract event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;

		private readonly IEventBus eventBus;

		protected ReadModelBase(IEventBus eventBus)
		{
			this.eventBus = eventBus;

			RegisterAtEventBus();
		}
		
		public abstract void Handle(AppointmentAdded    domainEvent);
		public abstract void Handle(AppointmentModified domainEvent);
		public abstract void Handle(AppointmentRemoved  domainEvent);



		// TODO: richtig so!?!?

		private bool disposed = false;
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		 
		~ReadModelBase()
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

		

		private void RegisterAtEventBus ()
		{
			eventBus.RegisterEventHandler<AppointmentAdded>(this);
			eventBus.RegisterEventHandler<AppointmentModified>(this);
			eventBus.RegisterEventHandler<AppointmentRemoved>(this);
		}

		private void DeregisterAtEventBus ()
		{
			eventBus.DeregisterEventHandler<AppointmentAdded>(this);
			eventBus.DeregisterEventHandler<AppointmentModified>(this);
			eventBus.DeregisterEventHandler<AppointmentRemoved>(this);
		}
	}
}
