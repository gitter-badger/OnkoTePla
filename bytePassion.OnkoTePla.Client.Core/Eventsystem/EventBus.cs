using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{
	public class EventBus
	{

		private IDictionary<Type, IList<IDomainEventHandler<DomainEvent>>> eventHandler;		
		
		public EventBus()
		{
			eventHandler = new ConcurrentDictionary<Type, IList<IDomainEventHandler<DomainEvent>>>();
		}

		public void Subscribe(IDomainEventHandler<DomainEvent> domainEventHandler)
		{
			if (!(eventHandler.ContainsKey(domainEventHandler.HandledEventType)))
				eventHandler.Add(domainEventHandler.HandledEventType, new List<IDomainEventHandler<DomainEvent>>());

			eventHandler[domainEventHandler.HandledEventType].Add(domainEventHandler);
		}		

		public void Publish(DomainEvent @event)
		{
			if (!eventHandler.ContainsKey(@event.GetType())) 
				return;


			foreach (var domainEventHandler in eventHandler[@event.GetType()])
			{
				domainEventHandler.Handle(@event);
			}
		}
	}
}