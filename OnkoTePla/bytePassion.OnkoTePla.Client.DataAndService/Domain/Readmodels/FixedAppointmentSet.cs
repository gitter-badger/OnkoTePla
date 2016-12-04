using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.LabelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels
{
	public class FixedAppointmentSet
	{
		
		public FixedAppointmentSet(AggregateIdentifier identifier,
								   uint aggregateVersion,
								   IEnumerable<Appointment> appointments)
		{
			Identifier = identifier;
			AggregateVersion = aggregateVersion;
			Appointments = appointments;
		}

		public FixedAppointmentSet (ClientMedicalPracticeData medicalPractice,
									IClientPatientRepository patientsRepository,
									IClientLabelRepository labelRepository,
									IEnumerable<AppointmentTransferData> initialAppointmentData,
									uint aggregateVersion,
									AggregateIdentifier identifier,
									Action<string> errorCallback)
		{
			AggregateVersion = aggregateVersion;
			Identifier = identifier;

			var appointmentSet = new AppointmentSet(patientsRepository, labelRepository, initialAppointmentData,
													medicalPractice, errorCallback);
			Appointments = appointmentSet.AppointmentList;
		}

		public uint AggregateVersion { private set; get; }
		public AggregateIdentifier Identifier { get; }

		public IEnumerable<Appointment> Appointments { get; }	
	}
}
