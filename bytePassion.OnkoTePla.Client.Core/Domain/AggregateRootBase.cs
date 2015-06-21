﻿
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public abstract class AggregateRootBase<TIdentifier>
	{
		private readonly TIdentifier id;
		private readonly IList<DomainEvent> uncommitedChanges;

		protected AggregateRootBase(TIdentifier id, uint version)
		{
			this.id = id;
			Version = version;

			uncommitedChanges = new List<DomainEvent>();
		}

		public TIdentifier Id
		{
			get { return id; }
		}

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

		public void LoadFromEventStream(EventStream eventStream)
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
