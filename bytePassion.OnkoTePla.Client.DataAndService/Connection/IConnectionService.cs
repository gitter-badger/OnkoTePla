using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.Domain;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	public interface IConnectionService : IDisposable
	{
		event Action<ConnectionEvent> ConnectionEventInvoked;
		
		Address          ServerAddress    { get; }
		Address          ClientAddress    { get; }
		ConnectionStatus ConnectionStatus { get; }
		

		void TryConnect     (Address serverAddress, Address clientAddress, Action<string> errorCallback);
		void TryDebugConnect(Address serverAddress, Address clientAddress, Action<string> errorCallback);
		void TryDisconnect  (Action<string> errorCallback);

		void TryLogin (Action loginSuccessfulCallback,  ClientUserData user, string password, Action<string> errorCallback);
		void TryLogout(Action logoutSuccessfulCallback, ClientUserData user,                  Action<string> errorCallback);


		void RequestUserList (Action<IReadOnlyList<ClientUserData>> dataReceivedCallback,
							  Action<string> errorCallback);

		void RequestAccessablePractices(Action<IReadOnlyList<Guid>> dataReceivedCallback,
										Action<string> errorCallback);
		
		void RequestPatientList(Action<IReadOnlyList<Patient>> dataReceivedCallback, 								
								Action<string> errorCallback);

		void RequestAppointmentsOfADay(Action<IReadOnlyList<AppointmentTransferData>, AggregateIdentifier, uint> dataReceivedCallback, 
									   Date day, Guid medicalPracticeId, uint aggregateVersionLimit,
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