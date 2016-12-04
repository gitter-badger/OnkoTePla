using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Base;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Notification;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.LabelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Exceptions;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels
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
											IClientLabelRepository labelRepository,
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

			var initialAppointmentList = initialAppointmentData.ToList();
			
			appointmentSet = new AppointmentSet(patientsRepository, labelRepository, 
												initialAppointmentList, medicalPractice, 
												errorCallback);			
		}		

		public uint AggregateVersion { private set; get; }
		public AggregateIdentifier Identifier { get; }		

		public IEnumerable<Appointment> Appointments
		{
			get { return appointmentSet.AppointmentList; }
		}

		public override void Process(AppointmentAdded domainEvent)
		{
			if (domainEvent.AggregateId != Identifier) return;

			if (AggregateVersion != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentAdded @readmodel");
			
			appointmentSet.AddAppointment(domainEvent.PatientId, 
										  domainEvent.Description, 
										  domainEvent.StartTime, 
										  domainEvent.EndTime, 
										  domainEvent.AggregateId.Date, 
										  domainEvent.TherapyPlaceId,
										  domainEvent.AppointmentId, 
										  domainEvent.LabelId,
										  errorCallback);	

			AggregateVersion = domainEvent.AggregateVersion + 1;
		}

		public override void Process (AppointmentReplaced domainEvent)
		{
			if (domainEvent.AggregateId != Identifier) return;

			if (AggregateVersion != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentReplaced @readmodel");

			appointmentSet.ReplaceAppointment(domainEvent.NewDescription,
											  domainEvent.NewDate,
											  domainEvent.NewStartTime,
											  domainEvent.NewEndTime,
											  domainEvent.NewTherapyPlaceId,
											  domainEvent.NewLabelId,
											  domainEvent.OriginalAppointmendId, 
											  errorCallback);

			AggregateVersion = domainEvent.AggregateVersion + 1;
		}
		 
		public override void Process (AppointmentDeleted domainEvent)
		{
			if (domainEvent.AggregateId != Identifier) return;

			if (AggregateVersion != domainEvent.AggregateVersion)
				throw new VersionNotApplicapleException("@handle appointmentDeleted @readmodel");

			appointmentSet.DeleteAppointment(domainEvent.RemovedAppointmentId);

			AggregateVersion = domainEvent.AggregateVersion + 1;
		}		
	}
}
