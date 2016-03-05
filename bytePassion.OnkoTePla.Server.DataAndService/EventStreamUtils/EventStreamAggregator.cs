using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Exceptions;

namespace bytePassion.OnkoTePla.Server.DataAndService.EventStreamUtils
{
	internal class EventStreamAggregator
	{

		public EventStreamAggregator(EventStream<Guid> eventStreamOfAPatient)
		{
			AggregateVersion = 0;
			var appointmentList = new List<AppointmentTransferData>(eventStreamOfAPatient.EventCount / 2);

			foreach (var domainEvent in eventStreamOfAPatient.Events)
			{				
				var appointmentAddedEvent = domainEvent as AppointmentAdded;
				if (appointmentAddedEvent != null)
				{
					HandleAddedEvent(appointmentAddedEvent, appointmentList);					
					continue;
				}

				var appointmentReplacedEvent = domainEvent as AppointmentReplaced;
				if (appointmentReplacedEvent != null)
				{
					HandleReplacedEvent(appointmentReplacedEvent, appointmentList);					
					continue;
				}

				var appointmentDeletedEvent = domainEvent as AppointmentDeleted;
				if (appointmentDeletedEvent != null)
				{
					HandleDeletedEvent(appointmentDeletedEvent, appointmentList);					
					continue;
				}

				throw new Exception();
			}

			AppointmentData = appointmentList;
		}

		public EventStreamAggregator(EventStream<AggregateIdentifier> eventStreamOfADay)
		{
			AggregateVersion = 0;
			var appointmentList = new List<AppointmentTransferData>(eventStreamOfADay.EventCount / 2);

			foreach (var domainEvent in eventStreamOfADay.Events)
			{
				if (AggregateVersion != domainEvent.AggregateVersion)
					throw new VersionNotApplicapleException();				

				var appointmentAddedEvent = domainEvent as AppointmentAdded;
				if (appointmentAddedEvent != null) {
					HandleAddedEvent(appointmentAddedEvent, appointmentList);
					AggregateVersion = appointmentAddedEvent.AggregateVersion + 1;
					continue;
				}

				var appointmentReplacedEvent = domainEvent as AppointmentReplaced;
				if (appointmentReplacedEvent != null)
				{
					HandleReplacedEvent(appointmentReplacedEvent, appointmentList);
					AggregateVersion = appointmentReplacedEvent.AggregateVersion + 1;
					continue;					
				}

				var appointmentDeletedEvent = domainEvent as AppointmentDeleted;
				if (appointmentDeletedEvent != null)
				{
					HandleDeletedEvent(appointmentDeletedEvent, appointmentList);
					AggregateVersion = appointmentDeletedEvent.AggregateVersion + 1;
					continue;
				}

				throw new Exception();
			}

			AppointmentData = appointmentList;
		}
		
		public IReadOnlyList<AppointmentTransferData> AppointmentData { get; } 
		public uint AggregateVersion { get; }

		private static void HandleAddedEvent(AppointmentAdded addedEvent, ICollection<AppointmentTransferData> appointmentList)
		{
			appointmentList.Add(new AppointmentTransferData(addedEvent.PatientId,
															addedEvent.Description,
															addedEvent.AggregateId.Date,
															addedEvent.StartTime,
															addedEvent.EndTime,
															addedEvent.TherapyPlaceId,
															addedEvent.AppointmentId,
															addedEvent.AggregateId.MedicalPracticeId));
			
		}
		
		private static void HandleReplacedEvent(AppointmentReplaced replacedEvent, ICollection<AppointmentTransferData> appointmentList)
		{
			var originalAppointment = appointmentList.First(appointment => appointment.Id == replacedEvent.OriginalAppointmendId);

			appointmentList.Remove(originalAppointment);
			appointmentList.Add(new AppointmentTransferData(originalAppointment.PatientId,
														    replacedEvent.NewDescription,
															replacedEvent.NewDate,
															replacedEvent.NewStartTime,
															replacedEvent.NewEndTime,
															replacedEvent.NewTherapyPlaceId,
															originalAppointment.Id,
															originalAppointment.MedicalPracticeId));			
		}

		private static void HandleDeletedEvent(AppointmentDeleted deletedEvent, ICollection<AppointmentTransferData> appointmentList)
		{
			var appointmentToDelete = appointmentList.First(appointment => appointment.Id == deletedEvent.RemovedAppointmentId);
			appointmentList.Remove(appointmentToDelete);			
		}
	}
}
