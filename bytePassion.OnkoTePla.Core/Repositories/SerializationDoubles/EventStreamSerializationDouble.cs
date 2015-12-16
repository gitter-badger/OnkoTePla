using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using System.Collections.Generic;
using System.Linq;


namespace bytePassion.OnkoTePla.Core.Repositories.SerializationDoubles
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

