using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles
{
	public class EventStreamSerializationDouble
	{

		public EventStreamSerializationDouble()
		{			
		}	

		public EventStreamSerializationDouble (EventStream<AggregateIdentifier> eventStream)
		{
			Id = new AggregateIdentifierSerializationDouble(eventStream.Id);
			Events = eventStream.Events.Select(DomainEventSerializationDouble.GetDomainEventSerializationDouble);
		}

		public AggregateIdentifierSerializationDouble      Id     { get; set; }
		public IEnumerable<DomainEventSerializationDouble> Events { get; set; }

		public EventStream<AggregateIdentifier> GetEventStream()
		{
			return new EventStream<AggregateIdentifier>(Id.GetAggregateIdentifier(),
														Events.Select(eventDouble => eventDouble.GetDomainEvent()));
		} 
	}
}

