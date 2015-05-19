using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{

	public class EventBus : IEventBus
	{		
		private readonly IDomainEventHandlerCollection eventHandlerRepository;		
		
		public EventBus()
		{			
			eventHandlerRepository = new DomainEventHandlerCollection();
		}


		public void Subscribe<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler) 
			where TDomainEvent : DomainEvent
		{
			eventHandlerRepository.Add(domainEventHandler);
		}
		
	
		public void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent
		{
			var eventHandlerList = eventHandlerRepository.GetAllDomainEventHandlersFor<TDomainEvent>();

			if (eventHandlerList == null) 
				return;

			foreach (var domainEventHandler in eventHandlerList)
			{
				domainEventHandler.Handle(@event);
			}
		}
	}
}