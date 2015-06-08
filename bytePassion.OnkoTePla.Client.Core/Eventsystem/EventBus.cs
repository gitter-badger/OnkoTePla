using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{

	public class EventBus : IEventBus
	{		
		private readonly IDomainEventHandlerCollection eventHandlerCollection;		
		
		public EventBus()
		{			
			eventHandlerCollection = new DomainEventHandlerCollection();
		}


		public void DeregisterEventHandler<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler) where TDomainEvent : DomainEvent
		{
			eventHandlerCollection.Remove(domainEventHandler);
		}

		public void RegisterEventHandler<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler) 
			where TDomainEvent : DomainEvent
		{
			eventHandlerCollection.Add(domainEventHandler);
		}
		
	
		public void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent
		{
			var eventHandlerList = eventHandlerCollection.GetAllDomainEventHandlersFor<TDomainEvent>();

			if (eventHandlerList == null) 
				return;

			foreach (var domainEventHandler in eventHandlerList)
			{
				domainEventHandler.Handle(@event);
			}
		}
	}
}