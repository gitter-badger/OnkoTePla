using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class EventStream
	{
		private readonly AggregateIdentifier id;
		private readonly List<DomainEvent> events; 

		public EventStream(AggregateIdentifier id, IEnumerable<DomainEvent> initialEventStream = null)
		{
			this.id = id;
			events = initialEventStream == null ? new List<DomainEvent>() : initialEventStream.ToList();
		}


		public AggregateIdentifier    Id     { get { return id;              }}
		public IEnumerable<DomainEvent> Events { get { return events.ToList(); }}

		public void AddEvents(IEnumerable<DomainEvent> newEvents)
		{
			events.AddRange(newEvents);
		}

		public int EventCount
		{
			get { return events.Count; }			
		}
	}
}