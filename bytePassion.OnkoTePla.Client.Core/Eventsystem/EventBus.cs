using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{

	public class EventBus : IEventBus
	{		
		private readonly DomainEventHandlerRepository eventHandlerRepository;		
		
		public EventBus()
		{			
			eventHandlerRepository = new DomainEventHandlerRepository();
		}


		public void Subscribe<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler) 
			where TDomainEvent : DomainEvent
		{
			eventHandlerRepository.Add(domainEventHandler);
		}
		
	
		public void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent
		{
			var eventHandlerList = eventHandlerRepository.GetAllDomainEventHandlers<TDomainEvent>();

			if (eventHandlerList == null) 
				return;

			foreach (var domainEventHandler in eventHandlerList)
			{
				domainEventHandler.Handle(@event);
			}
		}
	}
}