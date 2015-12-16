using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Exceptions;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Core.Readmodels
{
    public class AppointmentsOfADayReadModel : ReadModelBase
	{

		public override event EventHandler<AppointmentChangedEventArgs> AppointmentChanged
		{
			add    { appointmentSet.ObservableAppointments.AppointmentChanged += value; }
			remove { appointmentSet.ObservableAppointments.AppointmentChanged -= value; }
		}

		private readonly AppointmentSet appointmentSet;

		public AppointmentsOfADayReadModel (IEventBus eventBus, 
								           IConfigurationReadRepository config, 
								           IPatientReadRepository patientsRepository,
										   AggregateIdentifier identifier)
			: base(eventBus)
		{			
			Identifier = identifier;

			appointmentSet = new AppointmentSet(patientsRepository, config);			
		}

		public uint AggregateVersion { private set; get; }
		public AggregateIdentifier Identifier { get; }

		public void LoadFromEventStream (EventStream<AggregateIdentifier> eventStream)
		{			
			foreach (var domainEvent in eventStream.Events)			
				(this as dynamic).Process(Converter.ChangeTo(domainEvent, domainEvent.GetType()));			
		}

		public IEnumerable<Appointment> Appointments
		{
			get { return appointmentSet.AppointmentList; }
		}

		public override void Process(AppointmentAdded domainEvent)
		{
			if (domainEvent.AggregateId != Identifier) return;

			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentAdded @readmodel");
			
			appointmentSet.AddAppointment(domainEvent.AggregateId.MedicalPracticeId,
										  domainEvent.AggregateId.PracticeVersion,
										  domainEvent.CreateAppointmentData);	

			AggregateVersion = domainEvent.AggregateVersion;
		}

		public override void Process (AppointmentReplaced domainEvent)
		{
			if (domainEvent.AggregateId != Identifier) return;

			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentReplaced @readmodel");

			appointmentSet.ReplaceAppointment(domainEvent.AggregateId.MedicalPracticeId,
											  domainEvent.AggregateId.PracticeVersion,
											  domainEvent.NewDescription,
											  domainEvent.NewDate,
											  domainEvent.NewStartTime,
											  domainEvent.NewEndTime,
											  domainEvent.NewTherapyPlaceId,
											  domainEvent.OriginalAppointmendId);

			AggregateVersion = domainEvent.AggregateVersion;
		}
		 
		public override void Process (AppointmentDeleted domainEvent)
		{
			if (domainEvent.AggregateId != Identifier) return;

			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentDeleted @readmodel");

			appointmentSet.DeleteAppointment(domainEvent.RemovedAppointmentId);

			AggregateVersion = domainEvent.AggregateVersion;
		}		
	}
}
