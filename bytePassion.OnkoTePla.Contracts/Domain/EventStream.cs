using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
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
			var appointmentAddedEvent = domainEvent as AppointmentAdded;
			if (appointmentAddedEvent != null)
			{
				return TryToAddEvent(appointmentAddedEvent);				
			}

			var appointmentReplacedEvent = domainEvent as AppointmentReplaced;
			if (appointmentReplacedEvent != null)
			{
				return TryToReplaceEvent(appointmentReplacedEvent);				
			}

			var appointmentDeletedEvent = domainEvent as AppointmentDeleted;
			if (appointmentDeletedEvent != null)
			{
				return TryToDeleteEvent(appointmentDeletedEvent);				
			}

			throw new ArgumentException();
		}

		private bool TryToAddEvent(AppointmentAdded addedEvent)
		{
			if (eventStreamAggregator.AppointmentData.Any(appointment => appointment.Id == addedEvent.AppointmentId))
				return false;

			return eventStreamAggregator
					.AppointmentData
					.Where(appointment => appointment.TherapyPlaceId == addedEvent.TherapyPlaceId)								 
					.All(appointment => (appointment.StartTime <= addedEvent.StartTime || appointment.StartTime >= addedEvent.EndTime) &&
										(appointment.EndTime   <= addedEvent.StartTime || appointment.EndTime   >= addedEvent.EndTime));
		}

		private bool TryToReplaceEvent(AppointmentReplaced replacedEvent)
		{
			if (eventStreamAggregator.AppointmentData.All(appointment => appointment.Id != replacedEvent.OriginalAppointmendId))
				return false;

			return eventStreamAggregator
					.AppointmentData
					.Where(appointment => appointment.TherapyPlaceId == replacedEvent.NewTherapyPlaceId)
					.Where(appointment => appointment.Id != replacedEvent.OriginalAppointmendId)
					.All(appointment => (appointment.StartTime <= replacedEvent.NewStartTime || appointment.StartTime >= replacedEvent.NewEndTime) &&
										(appointment.EndTime   <= replacedEvent.NewStartTime || appointment.EndTime   >= replacedEvent.NewEndTime));
			
		}

		private bool TryToDeleteEvent(AppointmentDeleted deletedEvent)
		{
			return eventStreamAggregator.AppointmentData.Any(appointment => appointment.Id == deletedEvent.RemovedAppointmentId);
		}
	}
}