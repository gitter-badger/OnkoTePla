using System;
using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Client.Core.Exceptions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class AppointmentsOfADayReadModel : IDisposable, IDomainEventHandler<AppointmentAdded>, 
												            IDomainEventHandler<AppointmentModified>, 
												            IDomainEventHandler<AppointmentRemoved>
	{

		public event EventHandler<AppointmentChangedEventArgs> AppointmentChanged; 

		private readonly IEventBus eventBus;
		private readonly IConfigurationReadRepository config;
		private readonly IPatientReadRepository patientsRepository;
		private readonly AggregateIdentifier identifier;

		private readonly IList<Appointment> appointments;

		public AppointmentsOfADayReadModel(IEventBus eventBus, 
								           IConfigurationReadRepository config, 
								           IPatientReadRepository patientsRepository,
										   AggregateIdentifier identifier)
		{
			this.eventBus = eventBus;
			this.config = config;
			this.patientsRepository = patientsRepository;
			this.identifier = identifier;

			appointments = new List<Appointment>();

			RegisterAtEventBus();
		}

		public uint AggregateVersion { private set; get; }
		public AggregateIdentifier Identifier { get { return identifier; }}

		public void LoadFromEventStream(EventStream eventStream)
		{			
			foreach (var domainEvent in eventStream.Events)			
				(this as dynamic).Handle(Converter.ChangeTo(domainEvent, domainEvent.GetType()));			
		}

		public IEnumerable<Appointment> Appointments
		{
			get { return appointments; }
		}

		public void Handle(AppointmentAdded domainEvent)
		{
			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentAdded @readmodel");

			var medicalPractice = config.GetMedicalPracticeByIdAndVersion(domainEvent.AggregateId.MedicalPracticeId, 
																		  domainEvent.AggregateId.PracticeVersion);

			var appointment = new Appointment(patientsRepository.GetPatientById(domainEvent.PatientId), 
											  medicalPractice.GetTherapyPlaceById(domainEvent.TherapyPlaceId),
											  medicalPractice.GetRoomById(domainEvent.RoomId),
											  domainEvent.Day,
											  domainEvent.StartTime,
											  domainEvent.EndTime);
			
			appointments.Add(appointment);
			
			var handler = AppointmentChanged;
			if (handler != null)
				handler(this, new AppointmentChangedEventArgs(appointment, ChangeAction.Added));

			AggregateVersion = domainEvent.AggregateVersion;
		}

		public void Handle (AppointmentModified domainEvent)
		{
			throw new NotImplementedException();
		}

		public void Handle (AppointmentRemoved domainEvent)
		{
			throw new NotImplementedException();
		}


		public void Dispose()
		{
			DeregisterAtEventBus();
		}

		private void RegisterAtEventBus()
		{
			eventBus.RegisterEventHandler<AppointmentAdded>   (this);
			eventBus.RegisterEventHandler<AppointmentModified>(this);
			eventBus.RegisterEventHandler<AppointmentRemoved> (this);
		}

		private void DeregisterAtEventBus()
		{
			eventBus.DeregisterEventHandler<AppointmentAdded>   (this);
			eventBus.DeregisterEventHandler<AppointmentModified>(this);
			eventBus.DeregisterEventHandler<AppointmentRemoved> (this);
		}		
	}
}
