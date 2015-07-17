using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class EventStream<TIdentifier>
	{
		private readonly TIdentifier id;
		private readonly List<DomainEvent> events; 

		public EventStream(TIdentifier id, IEnumerable<DomainEvent> initialEventStream = null)
		{
			this.id = id;
			events = initialEventStream == null ? new List<DomainEvent>() : initialEventStream.ToList();
		}


		public TIdentifier              Id     { get { return id;              }}
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