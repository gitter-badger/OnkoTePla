using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Core.Eventsystem;

namespace bytePassion.OnkoTePla.Communication.NetworkMessageBus
{
	public class ServerEventBus : DisposingObject, IServerEventBus
	{								
		public void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent
		{
			throw new NotImplementedException();
		}

		protected override void CleanUp ()
		{

		}
	}
}
