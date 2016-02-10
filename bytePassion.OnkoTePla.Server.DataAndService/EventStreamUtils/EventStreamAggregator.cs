using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Exceptions;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;

namespace bytePassion.OnkoTePla.Server.DataAndService.EventStreamUtils
{
	internal class EventStreamAggregator
	{
		public EventStreamAggregator(EventStream<AggregateIdentifier> eventStreamOfADay, uint eventStreamLimit)
		{
			AggregateVersion = 0;
			var appointmentList = new List<AppointmentTransferData>(eventStreamOfADay.EventCount / 2);

			foreach (var domainEvent in eventStreamOfADay.Events)
			{
				if (AggregateVersion != domainEvent.AggregateVersion)
					throw new VersionNotApplicapleException();

				if (domainEvent.AggregateVersion > eventStreamLimit)
					break;

				var appointmentAddedEvent = domainEvent as AppointmentAdded;
				if (appointmentAddedEvent != null) {
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
		
		public IReadOnlyList<AppointmentTransferData> AppointmentData { get; } 
		public uint AggregateVersion { get; private set; }

		private void HandleAddedEvent(AppointmentAdded addedEvent, ICollection<AppointmentTransferData> appointmentList)
		{
			appointmentList.Add(new AppointmentTransferData(addedEvent.PatientId,
															addedEvent.CreateAppointmentData.Description,
															addedEvent.AggregateId.Date,
															addedEvent.CreateAppointmentData.StartTime,
															addedEvent.CreateAppointmentData.EndTime,
															addedEvent.CreateAppointmentData.TherapyPlaceId,
															addedEvent.CreateAppointmentData.AppointmentId));
			AggregateVersion = addedEvent.AggregateVersion + 1;
		}
		
		private void HandleReplacedEvent(AppointmentReplaced replacedEvent, ICollection<AppointmentTransferData> appointmentList)
		{
			var originalAppointment = appointmentList.First(appointment => appointment.Id == replacedEvent.OriginalAppointmendId);

			appointmentList.Remove(originalAppointment);
			appointmentList.Add(new AppointmentTransferData(originalAppointment.PatientId,
														    replacedEvent.NewDescription,
															replacedEvent.NewDate,
															replacedEvent.NewStartTime,
															replacedEvent.NewEndTime,
															replacedEvent.NewTherapyPlaceId,
															originalAppointment.Id));
			AggregateVersion = replacedEvent.AggregateVersion + 1;
		}

		private void HandleDeletedEvent(AppointmentDeleted deletedEvent, ICollection<AppointmentTransferData> appointmentList)
		{
			var appointmentToDelete = appointmentList.First(appointment => appointment.Id == deletedEvent.RemovedAppointmentId);
			appointmentList.Remove(appointmentToDelete);
			AggregateVersion = deletedEvent.AggregateVersion + 1;
		}
	}
}
