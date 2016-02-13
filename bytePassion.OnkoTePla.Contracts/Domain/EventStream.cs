using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Contracts.Domain
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

		public void AddEvent(DomainEvent newEvent)
		{
			events.Add(newEvent);
		}
		
		public int EventCount
		{
			get { return events.Count; }			
		}
	}
}