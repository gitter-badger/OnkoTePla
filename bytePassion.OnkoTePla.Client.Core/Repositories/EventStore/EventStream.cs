using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class EventStream<TIdentifier>
	{
		private readonly List<DomainEvent> events; 

		public EventStream(TIdentifier id, IEnumerable<DomainEvent> initialEventStream = null)
		{
			Id = id;
			events = initialEventStream?.ToList() ?? new List<DomainEvent>();
		}


		public TIdentifier              Id     { get; }
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