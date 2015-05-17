
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{
	public class EventSourcingSystem
	{
		public EventSourcingSystem()
		{
	
		}

		public void Subscribe<TEvent> (EventType eventType, IDomainEventHandler<TEvent> domainEventHandler)
		{

		}

		public void Subscribe<TEvent> (EventGroup eventGroup, IDomainEventHandler<TEvent> domainEventHandler)
		{
			
		}

		public void Publish(Event newEvent)
		{
			
		}
	}
}
