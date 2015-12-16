using System;
using System.Collections.Generic;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class AppointmentsOfAPatientReadModel : ReadModelBase
	{

		public override event EventHandler<AppointmentChangedEventArgs> AppointmentChanged
		{
			add    { appointmentSet.ObservableAppointments.AppointmentChanged += value; }
			remove { appointmentSet.ObservableAppointments.AppointmentChanged -= value; }
		}

		private readonly Patient patient;				

		private readonly AppointmentSet appointmentSet;

		public AppointmentsOfAPatientReadModel (Guid patientId,
												IEventBus eventBus,
												IConfigurationReadRepository config,
												IPatientReadRepository patientsRepository) 
			: base(eventBus)
		{						

			patient = patientsRepository.GetPatientById(patientId);

			appointmentSet = new AppointmentSet(patientsRepository, config);
		}		

		public void LoadFromEventStream (EventStream<Guid> eventStream)
		{
			foreach (var domainEvent in eventStream.Events)
				(this as dynamic).Process(Converter.ChangeTo(domainEvent, domainEvent.GetType()));
		}

		public IEnumerable<Appointment> Appointments
		{
			get { return appointmentSet.AppointmentList; }
		}

		public override void Process (AppointmentAdded domainEvent)
		{
			if (domainEvent.PatientId != patient.Id) return;			

			appointmentSet.AddAppointment(domainEvent.AggregateId.MedicalPracticeId,
										  domainEvent.AggregateId.PracticeVersion,
										  domainEvent.CreateAppointmentData);						
		}

		public override void Process (AppointmentReplaced domainEvent)
		{
			if (domainEvent.PatientId != patient.Id) return;

			appointmentSet.ReplaceAppointment(domainEvent.AggregateId.MedicalPracticeId,
											  domainEvent.AggregateId.PracticeVersion,
											  domainEvent.NewDescription,
											  domainEvent.NewDate,
											  domainEvent.NewStartTime,
											  domainEvent.NewEndTime,
											  domainEvent.NewTherapyPlaceId,
											  domainEvent.OriginalAppointmendId);
		}
		 
		public override void Process (AppointmentDeleted domainEvent)
		{
			if (domainEvent.PatientId != patient.Id) return;			
	
			appointmentSet.DeleteAppointment(domainEvent.RemovedAppointmentId);			
		}		
	}
}

