using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Core.Readmodels
{
	public class FixedAppointmentSet
	{
		private readonly AppointmentSet appointmentSet;	   

		public FixedAppointmentSet (ClientMedicalPracticeData medicalPractice,
									IPatientReadRepository patientsRepository,									
									EventStream<AggregateIdentifier> eventStream,
                                    uint? eventStreamVersionLimit)
		{
			MedicalPracticeVersion = eventStream.Id.PracticeVersion;
			appointmentSet = new AppointmentSet(patientsRepository);

			foreach (var domainEvent in eventStream.Events)
			{
				if (!eventStreamVersionLimit.HasValue || domainEvent.AggregateVersion <= eventStreamVersionLimit.Value)
				{
					if (domainEvent.GetType() == typeof(AppointmentAdded))
					{
						var addedEvent = (AppointmentAdded) domainEvent;
						appointmentSet.AddAppointment(addedEvent.CreateAppointmentData,
													  medicalPractice);
						AggregateVersion = domainEvent.AggregateVersion;
					}
					else if (domainEvent.GetType() == typeof (AppointmentReplaced))
					{
						var replacedEvent = (AppointmentReplaced) domainEvent;

						appointmentSet.ReplaceAppointment(replacedEvent.NewDescription,
														  replacedEvent.NewDate,
														  replacedEvent.NewStartTime,
														  replacedEvent.NewEndTime,
														  replacedEvent.NewTherapyPlaceId,
														  replacedEvent.OriginalAppointmendId,
														  medicalPractice);
						AggregateVersion = domainEvent.AggregateVersion;
					}
					else if (domainEvent.GetType() == typeof (AppointmentDeleted))
					{
						var deletedEvent = (AppointmentDeleted) domainEvent;
						appointmentSet.DeleteAppointment(deletedEvent.RemovedAppointmentId);
						AggregateVersion = domainEvent.AggregateVersion;
					}
					else
					{
						throw new Exception("internal error");
					}
				}
            }
		}
			
		public uint AggregateVersion       { get; private set; }
		public uint MedicalPracticeVersion { get; private set; }
					
		public IEnumerable<Appointment> Appointments
		{
			get { return appointmentSet.AppointmentList; }
		}	
	}
}
