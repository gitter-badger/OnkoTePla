using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{

	public class DomainEventHandlerRepository
	{			
		private readonly IDictionary<Type, IList> eventHandlerLists; 

		public DomainEventHandlerRepository()
		{
			eventHandlerLists = new Dictionary<Type, IList>();
		}

		public void Add<TEvent>(IDomainEventHandler<TEvent> newEventHandler) where TEvent : DomainEvent
		{
			if (!eventHandlerLists.ContainsKey(typeof(TEvent)))
				eventHandlerLists.Add(typeof(TEvent), new ArrayList());

			eventHandlerLists[typeof (TEvent)].Add(newEventHandler);
		}

		public IEnumerable<IDomainEventHandler<TEvent>> GetAllDomainEventHandlers<TEvent>() where TEvent : DomainEvent
		{
			if (!eventHandlerLists.ContainsKey(typeof(TEvent)))					
				return null;

			return eventHandlerLists[typeof(TEvent)].Cast<IDomainEventHandler<TEvent>>().ToList();
		} 
	}

}