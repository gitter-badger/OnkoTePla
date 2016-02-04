using System;
using System.Collections.Generic;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Core.Readmodels
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
		private readonly Func<Guid, uint, ClientMedicalPracticeData> practiceRepository;

		public AppointmentsOfAPatientReadModel (Guid patientId,
												IClientEventBus eventBus,
												Func<Guid, uint, ClientMedicalPracticeData> practiceRepository,
												IPatientReadRepository patientsRepository) 
			: base(eventBus)
		{
			this.practiceRepository = practiceRepository;

			patient = patientsRepository.GetPatientById(patientId);

			appointmentSet = new AppointmentSet(patientsRepository);
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

			appointmentSet.AddAppointment(domainEvent.CreateAppointmentData,
										  practiceRepository(domainEvent.AggregateId.MedicalPracticeId,
															 domainEvent.AggregateId.PracticeVersion));						
		}

		public override void Process (AppointmentReplaced domainEvent)
		{
			if (domainEvent.PatientId != patient.Id) return;

			appointmentSet.ReplaceAppointment(domainEvent.NewDescription,
											  domainEvent.NewDate,
											  domainEvent.NewStartTime,
											  domainEvent.NewEndTime,
											  domainEvent.NewTherapyPlaceId,
											  domainEvent.OriginalAppointmendId,
											  practiceRepository(domainEvent.AggregateId.MedicalPracticeId,
																 domainEvent.AggregateId.PracticeVersion));
		}
		 
		public override void Process (AppointmentDeleted domainEvent)
		{
			if (domainEvent.PatientId != patient.Id) return;			
	
			appointmentSet.DeleteAppointment(domainEvent.RemovedAppointmentId);			
		}		
	}
}

