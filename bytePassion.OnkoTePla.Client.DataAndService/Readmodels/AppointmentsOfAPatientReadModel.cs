using System;
using System.Collections.Generic;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Readmodels;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;

namespace bytePassion.OnkoTePla.Client.DataAndService.Readmodels
{
	public class AppointmentsOfAPatientReadModel : PatientReadModelBase
	{
		private readonly Guid patientId;

		public override event EventHandler<RawAppointmentChangedEventArgs> AppointmentChanged
		{
			add    { appointmentSet.ObservableAppointments.AppointmentChanged += value; }
			remove { appointmentSet.ObservableAppointments.AppointmentChanged -= value; }
		}
					
		private readonly RawAppointmentSet appointmentSet;		

		public AppointmentsOfAPatientReadModel (Guid patientId,
												IClientEventBus eventBus,
												IEnumerable<AppointmentTransferData> initialAppointmentData,
												IClientPatientRepository patientsRepository) 
			: base(eventBus)
		{
			this.patientId = patientId;
			

			appointmentSet = new RawAppointmentSet(initialAppointmentData);
		}

		public void LoadFromEventStream (EventStream<Guid> eventStream)
		{
			foreach (var domainEvent in eventStream.Events)
				(this as dynamic).Process(Converter.ChangeTo(domainEvent, domainEvent.GetType()));
		}

		public IEnumerable<AppointmentTransferData> Appointments
		{
			get { return appointmentSet.AppointmentList; }
		}

		public override void Process (AppointmentAdded domainEvent)
		{
			if (domainEvent.PatientId != patientId) return;			

			appointmentSet.AddAppointment(domainEvent.CreateAppointmentData);						
		}

		public override void Process (AppointmentReplaced domainEvent)
		{
			if (domainEvent.PatientId != patientId) return;

			appointmentSet.ReplaceAppointment(domainEvent.NewDescription,
											  domainEvent.NewDate,
											  domainEvent.NewStartTime,
											  domainEvent.NewEndTime,
											  domainEvent.NewTherapyPlaceId,
											  domainEvent.OriginalAppointmendId);
		}
		 
		public override void Process (AppointmentDeleted domainEvent)
		{
			if (domainEvent.PatientId != patientId) return;			
	
			appointmentSet.DeleteAppointment(domainEvent.RemovedAppointmentId);			
		}		
	}
}

