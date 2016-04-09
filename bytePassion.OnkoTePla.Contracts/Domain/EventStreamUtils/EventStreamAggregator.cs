using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Domain.Exceptions;

namespace bytePassion.OnkoTePla.Contracts.Domain.EventStreamUtils
{
	public class EventStreamAggregator<TIdent>
	{		
		public EventStreamAggregator(EventStream<TIdent> eventStream)
		{
			AppointmentData = new List<AppointmentTransferData>(eventStream.EventCount / 2);

			if (typeof(TIdent) == typeof(Guid))
			{
				Aggregate(eventStream as EventStream<Guid>);
			}
			else if (typeof (TIdent) == typeof (AggregateIdentifier))
			{
				Aggregate(eventStream as EventStream<AggregateIdentifier>);
			}
			else
			{
				throw new ArgumentException();
			}
		}

		private void Aggregate(EventStream<Guid> eventStreamOfAPatient)
		{
			AggregateVersion = 0;			

			foreach (var domainEvent in eventStreamOfAPatient.Events)
			{				
				var appointmentAddedEvent = domainEvent as AppointmentAdded;
				if (appointmentAddedEvent != null)
				{
					HandleAddedEvent(appointmentAddedEvent);					
					continue;
				}

				var appointmentReplacedEvent = domainEvent as AppointmentReplaced;
				if (appointmentReplacedEvent != null)
				{
					HandleReplacedEvent(appointmentReplacedEvent);					
					continue;
				}

				var appointmentDeletedEvent = domainEvent as AppointmentDeleted;
				if (appointmentDeletedEvent != null)
				{
					HandleDeletedEvent(appointmentDeletedEvent);					
					continue;
				}

				throw new Exception();
			}			
		}

		private void Aggregate (EventStream<AggregateIdentifier> eventStreamOfADay)
		{
			AggregateVersion = 0;			

			foreach (var domainEvent in eventStreamOfADay.Events)
			{
				if (AggregateVersion != domainEvent.AggregateVersion)
					throw new VersionNotApplicapleException();				

				var appointmentAddedEvent = domainEvent as AppointmentAdded;
				if (appointmentAddedEvent != null) {
					HandleAddedEvent(appointmentAddedEvent);
					AggregateVersion = appointmentAddedEvent.AggregateVersion + 1;
					continue;
				}

				var appointmentReplacedEvent = domainEvent as AppointmentReplaced;
				if (appointmentReplacedEvent != null)
				{
					HandleReplacedEvent(appointmentReplacedEvent);
					AggregateVersion = appointmentReplacedEvent.AggregateVersion + 1;
					continue;					
				}

				var appointmentDeletedEvent = domainEvent as AppointmentDeleted;
				if (appointmentDeletedEvent != null)
				{
					HandleDeletedEvent(appointmentDeletedEvent);
					AggregateVersion = appointmentDeletedEvent.AggregateVersion + 1;
					continue;
				}

				throw new Exception();
			}			
		}

		public IList<AppointmentTransferData> AppointmentData { get; }
		public uint AggregateVersion { get; private set; }


		public void AddEvent(DomainEvent domainEvent)
		{
			var addedEvent    = domainEvent as AppointmentAdded;    if (addedEvent    != null) { HandleAddedEvent(addedEvent);       return; }
			var replacedEvent = domainEvent as AppointmentReplaced; if (replacedEvent != null) { HandleReplacedEvent(replacedEvent); return; }
			var deletedEvent  = domainEvent as AppointmentDeleted;  if (deletedEvent  != null) { HandleDeletedEvent(deletedEvent);   return; }

			throw new ArgumentException();
		}

		private void HandleAddedEvent(AppointmentAdded addedEvent)
		{
			AppointmentData.Add(new AppointmentTransferData(addedEvent.PatientId,
															addedEvent.Description,
															addedEvent.AggregateId.Date,
															addedEvent.StartTime,
															addedEvent.EndTime,
															addedEvent.TherapyPlaceId,
															addedEvent.AppointmentId,
															addedEvent.AggregateId.MedicalPracticeId,
															addedEvent.LabelId));
			
		}
		
		private void HandleReplacedEvent(AppointmentReplaced replacedEvent)
		{
			var originalAppointment = AppointmentData.First(appointment => appointment.Id == replacedEvent.OriginalAppointmendId);

			AppointmentData.Remove(originalAppointment);
			AppointmentData.Add(new AppointmentTransferData(originalAppointment.PatientId,
														    replacedEvent.NewDescription,
															replacedEvent.NewDate,
															replacedEvent.NewStartTime,
															replacedEvent.NewEndTime,
															replacedEvent.NewTherapyPlaceId,
															originalAppointment.Id,
															originalAppointment.MedicalPracticeId,
															replacedEvent.NewLabelId));			
		}
		
		private void HandleDeletedEvent(AppointmentDeleted deletedEvent)
		{
			var appointmentToDelete = AppointmentData.First(appointment => appointment.Id == deletedEvent.RemovedAppointmentId);
			AppointmentData.Remove(appointmentToDelete);			
		}
	}
}
