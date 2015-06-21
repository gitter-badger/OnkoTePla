using System;
using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Bus;
using bytePassion.OnkoTePla.Client.Core.Exceptions;
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
		private readonly AggregateIdentifier identifier;

		private readonly AppointmentSet appointmentSet;

		public AppointmentsOfAPatientReadModel (Guid patientId,
												IEventBus eventBus,
												IConfigurationReadRepository config,
												IPatientReadRepository patientsRepository,
												AggregateIdentifier identifier) 
			: base(eventBus)
		{			
			this.identifier = identifier;

			patient = patientsRepository.GetPatientById(patientId);

			appointmentSet = new AppointmentSet(patientsRepository, config);

		}

		public uint AggregateVersion { private set; get; }
		public AggregateIdentifier Identifier { get { return identifier; } }

		public void LoadFromEventStream (EventStream eventStream)
		{
			foreach (var domainEvent in eventStream.Events)
				(this as dynamic).Handle(Converter.ChangeTo(domainEvent, domainEvent.GetType()));
		}

		public IEnumerable<Appointment> Appointments
		{
			get { return appointmentSet.AppointmentList; }
		}

		public override void Handle (AppointmentAdded domainEvent)
		{
			if (domainEvent.CreateAppointmentData.PatientId != patient.Id) return;

			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentAdded @readmodel");		

			appointmentSet.AddAppointment(domainEvent.AggregateId.MedicalPracticeId,
										  domainEvent.AggregateId.PracticeVersion,
										  domainEvent.CreateAppointmentData);			

			AggregateVersion = domainEvent.AggregateVersion;
		}

		public override void Handle (AppointmentModified domainEvent)
		{			
			throw new NotImplementedException();
		}

		public override void Handle (AppointmentRemoved domainEvent)
		{			
			throw new NotImplementedException();
		}		
	}
}

