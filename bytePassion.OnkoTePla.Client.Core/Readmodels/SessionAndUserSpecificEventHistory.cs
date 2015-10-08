using System;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class SessionAndUserSpecificEventHistory : ReadModelBase
	{
		public SessionAndUserSpecificEventHistory(IEventBus eventBus) : base(eventBus)
		{
			
		}		

		public override void Process(AppointmentAdded domainEvent)
		{
			throw new NotImplementedException();
		}

		public override void Process(AppointmentReplaced domainEvent)
		{
			throw new NotImplementedException();
		}

		public override void Process(AppointmentDeleted domainEvent)
		{
			throw new NotImplementedException();
		}

		public override event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;
	}
}
