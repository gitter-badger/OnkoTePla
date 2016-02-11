using System;
using System.Collections.Generic;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Exceptions;
using bytePassion.OnkoTePla.Core.Readmodels;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;

namespace bytePassion.OnkoTePla.Client.DataAndService.Readmodels
{
	public class AppointmentsOfADayReadModel : DayReadModelBase
	{
		private readonly Action<string> errorCallback;

		public override event EventHandler<AppointmentChangedEventArgs> AppointmentChanged
		{
			add    { appointmentSet.ObservableAppointments.AppointmentChanged += value; }
			remove { appointmentSet.ObservableAppointments.AppointmentChanged -= value; }
		}
		
		private readonly AppointmentSet appointmentSet;

		public AppointmentsOfADayReadModel (IClientEventBus eventBus,
										    IClientPatientRepository patientsRepository,
											ClientMedicalPracticeData medicalPractice, 								            
											IEnumerable<AppointmentTransferData> initialAppointmentData,
											AggregateIdentifier identifier,
											uint initialAggregateVersion,
											Action<string> errorCallback)
			: base(eventBus)
		{
			this.errorCallback = errorCallback;

			AggregateVersion = initialAggregateVersion;
			Identifier = identifier;
			appointmentSet = new AppointmentSet(patientsRepository, initialAppointmentData, 
												medicalPractice, errorCallback);			
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
			
			appointmentSet.AddAppointment(domainEvent.CreateAppointmentData, errorCallback);	

			AggregateVersion = domainEvent.AggregateVersion;
		}

		public override void Process (AppointmentReplaced domainEvent)
		{
			if (domainEvent.AggregateId != Identifier) return;

			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentReplaced @readmodel");

			appointmentSet.ReplaceAppointment(domainEvent.NewDescription,
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
