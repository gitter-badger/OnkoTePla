using System;
using System.Collections.Generic;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Exceptions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class AppointmentsOfADayReadModel : ReadModelBase
	{

		public override event EventHandler<AppointmentChangedEventArgs> AppointmentChanged
		{
			add    { appointmentSet.ObservableAppointments.AppointmentChanged += value; }
			remove { appointmentSet.ObservableAppointments.AppointmentChanged -= value; }
		}
		
		private readonly AggregateIdentifier identifier;
		private readonly AppointmentSet      appointmentSet;

		public AppointmentsOfADayReadModel (IEventBus eventBus, 
								           IConfigurationReadRepository config, 
								           IPatientReadRepository patientsRepository,
										   AggregateIdentifier identifier)
			: base(eventBus)
		{			
			this.identifier = identifier;

			appointmentSet = new AppointmentSet(patientsRepository, config);			
		}

		public uint AggregateVersion { private set; get; }
		public AggregateIdentifier Identifier { get { return identifier; }}

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
			if (domainEvent.AggregateId != identifier) return;

			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentAdded @readmodel");
			
			appointmentSet.AddAppointment(domainEvent.AggregateId.MedicalPracticeId,
										  domainEvent.AggregateId.PracticeVersion,
										  domainEvent.CreateAppointmentData);	

			AggregateVersion = domainEvent.AggregateVersion;
		}

		public override void Process (AppointmentReplaced domainEvent)
		{
			if (domainEvent.AggregateId != identifier) return;

			throw new NotImplementedException();
		}
		 
		public override void Process (AppointmentDeleted domainEvent)
		{
			if (domainEvent.AggregateId != identifier) return;

			appointmentSet.DeleteAppointment(domainEvent.RemovedAppointmentId);

			AggregateVersion = domainEvent.AggregateVersion;
		}		
	}
}
