
using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public class AggregateRootBase
	{
		private readonly Guid id;
		private readonly IList<DomainEvent> uncommitedChanges;

		public AggregateRootBase(Guid id, uint version)
		{
			this.id = id;
			Version = version;

			uncommitedChanges = new List<DomainEvent>();
		}

		public Guid Id
		{
			get { return id; }
		}

		public uint Version
		{
			protected set; get;
		}

		public IEnumerable<DomainEvent> GetUncommitedChanges()
		{
			return uncommitedChanges;
		}

		public void MarkChangesAsCommited()
		{
			uncommitedChanges.Clear();
		}

		public void LoadFromEventStream(IEnumerable<DomainEvent> eventStream)
		{
			foreach (var domainEvent in eventStream)
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
			(this as dynamic).Apply(@event);
			
			if (isNew) uncommitedChanges.Add(@event);			
		}      
	}
}
