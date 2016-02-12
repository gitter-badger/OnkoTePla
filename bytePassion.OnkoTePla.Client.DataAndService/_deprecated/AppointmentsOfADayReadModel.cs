//using System;
//using System.Collections.Generic;
//using bytePassion.Lib.Utils;
//using bytePassion.OnkoTePla.Contracts.Appointments;
//using bytePassion.OnkoTePla.Contracts.Infrastructure;
//using bytePassion.OnkoTePla.Core.Domain;
//using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
//using bytePassion.OnkoTePla.Core.Domain.Events;
//using bytePassion.OnkoTePla.Core.Eventsystem;
//using bytePassion.OnkoTePla.Core.Exceptions;
//using bytePassion.OnkoTePla.Core.Repositories.EventStore;
//using bytePassion.OnkoTePla.Core.Repositories.Patients;
//
//
//namespace bytePassion.OnkoTePla.Core.Readmodels
//{
//	public class AppointmentsOfADayReadModel : ReadModelBase
//	{		
//		public override event EventHandler<AppointmentChangedEventArgs> AppointmentChanged
//		{
//			add    { appointmentSet.ObservableAppointments.AppointmentChanged += value; }
//			remove { appointmentSet.ObservableAppointments.AppointmentChanged -= value; }
//		}
//
//		private readonly ClientMedicalPracticeData medicalPractice;
//		private readonly AppointmentSet appointmentSet;
//
//		public AppointmentsOfADayReadModel (IClientEventBus eventBus, 
//								            ClientMedicalPracticeData medicalPractice, 
//								            IPatientReadRepository patientsRepository,
//										    AggregateIdentifier identifier)
//			: base(eventBus)
//		{
//			this.medicalPractice = medicalPractice;
//			Identifier = identifier;
//
//			appointmentSet = new AppointmentSet(patientsRepository);			
//		}
//
//		public AppointmentsOfADayReadModel(IClientEventBus eventBus,
//										   ClientMedicalPracticeData medicalPractice,
//										   AppointmentSet initialAppointmentSet,
//										   AggregateIdentifier identifier)
//			: base(eventBus)
//		{
//			this.medicalPractice = medicalPractice;
//			Identifier = identifier;
//			appointmentSet = initialAppointmentSet;
//		}
//
//		public uint AggregateVersion { private set; get; }
//		public AggregateIdentifier Identifier { get; }
//
//		public void LoadFromEventStream (EventStream<AggregateIdentifier> eventStream)
//		{			
//			foreach (var domainEvent in eventStream.Events)			
//				(this as dynamic).Process(Converter.ChangeTo(domainEvent, domainEvent.GetType()));			
//		}
//
//		public IEnumerable<Appointment> Appointments
//		{
//			get { return appointmentSet.AppointmentList; }
//		}
//
//		public override void Process(AppointmentAdded domainEvent)
//		{
//			if (domainEvent.AggregateId != Identifier) return;
//
//			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
//				throw new VersionNotApplicapleException("@handle appointmentAdded @readmodel");
//			
//			appointmentSet.AddAppointment(domainEvent.CreateAppointmentData, medicalPractice);	
//
//			AggregateVersion = domainEvent.AggregateVersion;
//		}
//
//		public override void Process (AppointmentReplaced domainEvent)
//		{
//			if (domainEvent.AggregateId != Identifier) return;
//
//			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
//				throw new VersionNotApplicapleException("@handle appointmentReplaced @readmodel");
//
//			appointmentSet.ReplaceAppointment(domainEvent.NewDescription,
//											  domainEvent.NewDate,
//											  domainEvent.NewStartTime,
//											  domainEvent.NewEndTime,
//											  domainEvent.NewTherapyPlaceId,
//											  domainEvent.OriginalAppointmendId,
//											  medicalPractice);
//
//			AggregateVersion = domainEvent.AggregateVersion;
//		}
//		 
//		public override void Process (AppointmentDeleted domainEvent)
//		{
//			if (domainEvent.AggregateId != Identifier) return;
//
//			if (AggregateVersion + 1 != domainEvent.AggregateVersion)
//				throw new VersionNotApplicapleException("@handle appointmentDeleted @readmodel");
//
//			appointmentSet.DeleteAppointment(domainEvent.RemovedAppointmentId);
//
//			AggregateVersion = domainEvent.AggregateVersion;
//		}		
//	}
//}
