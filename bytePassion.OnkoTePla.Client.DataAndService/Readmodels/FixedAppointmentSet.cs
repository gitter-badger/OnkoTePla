using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Domain;

namespace bytePassion.OnkoTePla.Client.DataAndService.Readmodels
{
	public class FixedAppointmentSet
	{
		
		public FixedAppointmentSet(AggregateIdentifier identifier,
								   uint aggregateVersion,
								   IEnumerable<Appointment> appointments)
		{
			Identifier = identifier;
			AggregateVersion = aggregateVersion;
			Appointments = appointments;
		}

		public FixedAppointmentSet (ClientMedicalPracticeData medicalPractice,
									IClientPatientRepository patientsRepository,
									IEnumerable<AppointmentTransferData> initialAppointmentData,
									uint aggregateVersion,
									AggregateIdentifier identifier,
									Action<string> errorCallback)
		{
			AggregateVersion = aggregateVersion;
			Identifier = identifier;


			var appointmentSet = new AppointmentSet(patientsRepository, initialAppointmentData,
													medicalPractice, errorCallback);
			Appointments = appointmentSet.AppointmentList;
		}

		public uint AggregateVersion { private set; get; }
		public AggregateIdentifier Identifier { get; }

		public IEnumerable<Appointment> Appointments { get; }	
	}
}

#region oldCode

//			MedicalPracticeVersion = eventStream.Id.PracticeVersion;
//			appointmentSet = new AppointmentSet(patientsRepository);
//
//			foreach (var domainEvent in eventStream.Events)
//			{
//				if (!eventStreamVersionLimit.HasValue || domainEvent.AggregateVersion <= eventStreamVersionLimit.Value)
//				{
//					if (domainEvent.GetType() == typeof(AppointmentAdded))
//					{
//						var addedEvent = (AppointmentAdded) domainEvent;
//						appointmentSet.AddAppointment(addedEvent.CreateAppointmentData,
//													  medicalPractice);
//						AggregateVersion = domainEvent.AggregateVersion;
//					}
//					else if (domainEvent.GetType() == typeof (AppointmentReplaced))
//					{
//						var replacedEvent = (AppointmentReplaced) domainEvent;
//
//						appointmentSet.ReplaceAppointment(replacedEvent.NewDescription,
//														  replacedEvent.NewDate,
//														  replacedEvent.NewStartTime,
//														  replacedEvent.NewEndTime,
//														  replacedEvent.NewTherapyPlaceId,
//														  replacedEvent.OriginalAppointmendId,
//														  medicalPractice);
//						AggregateVersion = domainEvent.AggregateVersion;
//					}
//					else if (domainEvent.GetType() == typeof (AppointmentDeleted))
//					{
//						var deletedEvent = (AppointmentDeleted) domainEvent;
//						appointmentSet.DeleteAppointment(deletedEvent.RemovedAppointmentId);
//						AggregateVersion = domainEvent.AggregateVersion;
//					}
//					else
//					{
//						throw new Exception("internal error");
//					}
//				}
//            }

#endregion
