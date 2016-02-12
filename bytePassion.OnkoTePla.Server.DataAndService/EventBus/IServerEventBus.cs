using System;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Core.Eventsystem
{
	public interface IServerEventBus : IDisposable
	{		
		void Publish<TDomainEvent> (TDomainEvent @event) where TDomainEvent : DomainEvent;
	}
}