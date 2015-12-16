using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class FixedAppointmentSet
	{
		private readonly AppointmentSet appointmentSet;

		public FixedAppointmentSet (IConfigurationReadRepository config,
									IPatientReadRepository patientsRepository,									
									EventStream<AggregateIdentifier> eventStream,
                                    uint eventStreamVersionLimit)			
		{
			appointmentSet = new AppointmentSet(patientsRepository, config);

			foreach (var domainEvent in eventStream.Events)
			{
				if (domainEvent.AggregateVersion <= eventStreamVersionLimit)
				{
					if (domainEvent.GetType() == typeof(AppointmentAdded))
					{
						var addedEvent = (AppointmentAdded) domainEvent;
						appointmentSet.AddAppointment(addedEvent.AggregateId.MedicalPracticeId,
													  addedEvent.AggregateId.PracticeVersion,
													  addedEvent.CreateAppointmentData);
					}
					else if (domainEvent.GetType() == typeof (AppointmentReplaced))
					{
						var replacedEvent = (AppointmentReplaced) domainEvent;

						appointmentSet.ReplaceAppointment(replacedEvent.AggregateId.MedicalPracticeId,
														  replacedEvent.AggregateId.PracticeVersion,
														  replacedEvent.NewDescription,
														  replacedEvent.NewDate,
														  replacedEvent.NewStartTime,
														  replacedEvent.NewEndTime,
														  replacedEvent.NewTherapyPlaceId,
														  replacedEvent.OriginalAppointmendId);
					}
					else if (domainEvent.GetType() == typeof (AppointmentDeleted))
					{
						var deletedEvent = (AppointmentDeleted) domainEvent;
						appointmentSet.DeleteAppointment(deletedEvent.RemovedAppointmentId);
					}
					else
					{
						throw new Exception("internal error");
					}
				}
            }
		}
						
		public IEnumerable<Appointment> Appointments
		{
			get { return appointmentSet.AppointmentList; }
		}	
	}
}
