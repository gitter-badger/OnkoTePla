
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public abstract class AggregateRootBase<TIdentifier>
	{
		private readonly IList<DomainEvent> uncommitedChanges;

		protected AggregateRootBase(TIdentifier id, uint version)
		{
			Id = id;
			Version = version;

			uncommitedChanges = new List<DomainEvent>();
		}

		public TIdentifier Id { get; }

		public uint Version
		{
			protected set; get;
		}

		public IEnumerable<DomainEvent> GetUncommitedChanges()
		{
			var uncommitedEvents = uncommitedChanges.ToList();
			uncommitedChanges.Clear();
			return uncommitedEvents;
		}		

		public void LoadFromEventStream(EventStream<TIdentifier> eventStream)
		{
			foreach (var domainEvent in eventStream.Events)
			{
				ApplyChange(domainEvent, false);
				Version = domainEvent.AggregateVersion;
			}			
		}

		protected void ApplyChange<TDomainEvent> (TDomainEvent @event) where TDomainEvent : DomainEvent
		{
			ApplyChange(@event, true);
		}

		private void ApplyChange<TDomainEvent>(TDomainEvent @event, bool isNew) where TDomainEvent : DomainEvent
		{
			(this as dynamic).Apply(Converter.ChangeTo(@event, @event.GetType()));			
			
			if (isNew) uncommitedChanges.Add(@event);			
		}		
	}
}
