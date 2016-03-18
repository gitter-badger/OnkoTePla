using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Domain.EventStreamUtils;

namespace bytePassion.OnkoTePla.Contracts.Domain
{
	public class EventStream<TIdentifier>
	{
		private readonly List<DomainEvent> events;
		private readonly EventStreamAggregator<TIdentifier> eventStreamAggregator;
		
		public EventStream(TIdentifier id, IEnumerable<DomainEvent> initialEventStream = null)
		{
			Id = id;
			events = initialEventStream?.ToList() ?? new List<DomainEvent>();
			eventStreamAggregator = new EventStreamAggregator<TIdentifier>(this);
		}

		public TIdentifier              Id     { get; }
		public IEnumerable<DomainEvent> Events { get { return events.ToList(); }}
		
		public bool AddEvent(DomainEvent newEvent)
		{
			if (!IsEventApplicaple(newEvent))
				return false;

			eventStreamAggregator.AddEvent(newEvent);
			events.Add(newEvent);
			return true;
		}
		
		public int EventCount
		{
			get { return events.Count; }			
		}

		private bool IsEventApplicaple(DomainEvent domainEvent)
		{
			return true;
		}
	}
}