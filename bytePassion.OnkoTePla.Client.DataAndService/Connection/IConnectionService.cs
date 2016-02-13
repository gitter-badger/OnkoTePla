using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	public interface IConnectionService : IDisposable
	{
		event Action<ConnectionEvent> ConnectionEventInvoked;

		event Action<DomainEvent> NewDomainEventAvailable;
		event Action<Patient> NewPatientAvailable;
		event Action<Patient> UpdatedPatientAvailable;
		event Action<TherapyPlaceType> NewTherapyPlaceTypeAvailable;
		event Action<TherapyPlaceType> UpdatedTherapyPlaceTypeAvailable;

		Address          ServerAddress    { get; }
		Address          ClientAddress    { get; }
		ConnectionStatus ConnectionStatus { get; }
		

		void TryConnect     (Address serverAddress, Address clientAddress, Action<string> errorCallback);
		void TryDebugConnect(Address serverAddress, Address clientAddress, Action<string> errorCallback);
		void TryDisconnect  (Action<string> errorCallback);

		void TryLogin (Action loginSuccessfulCallback,  ClientUserData user, string password, Action<string> errorCallback);
		void TryLogout(Action logoutSuccessfulCallback, ClientUserData user,                  Action<string> errorCallback);

		void TryAddEvents(Action<bool> resultCallback, IReadOnlyList<DomainEvent> newEvents, Action<string> errorCallback);
		
		void RequestUserList (Action<IReadOnlyList<ClientUserData>> dataReceivedCallback,
							  Action<string> errorCallback);
		
		void RequestPatientList(Action<IReadOnlyList<Patient>> dataReceivedCallback, 								
								Action<string> errorCallback);

		void RequestAppointmentsOfADay(Action<IReadOnlyList<AppointmentTransferData>, AggregateIdentifier, uint> dataReceivedCallback, 
									   Date day, Guid medicalPracticeId, uint aggregateVersionLimit,
									   Action<string> errorCallback); 
	
		void RequestAppointmentsOfAPatient(Action<IReadOnlyList<AppointmentTransferData>> dataReceivedCallaback,
										   Guid patientId,
										   Action<string> errorCallback);
		
		void RequestMedicalPractice(Action<ClientMedicalPracticeData> dataReceivedCallback,
									Guid medicalPracticeId, uint medicalPracticeVersion,
									Action<string> errorCallback);

		void RequestPracticeVersionInfo(Action<uint> dataReceivedCallback, 
										Guid medicalPracticeId, Date day,
										Action<string> errorCallback);

		void RequestTherapyPlaceTypeList(Action<IReadOnlyList<TherapyPlaceType>> dataReceivedCallback,
										 Action<string> errorCallback);		
	}
}