using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public interface IConnectionService : IDisposable
	{
		event Action ConnectionStatusChanged;

		event Action<SessionInfo> NewSessionStarted;
		event Action<SessionInfo> SessionTerminated;
		event Action<SessionInfo> LoggedInUserUpdated;
		
		SessionInfo GetSessionInfo(ConnectionSessionId id);
		bool IsConnectionActive { get; }

		void InitiateCommunication (Address serverAddress);
		void StopCommunication ();

		void SendEventNotification(DomainEvent domainEvent);
		void SendPatientAddedNotification(Patient newPatient);
		void SendPatientUpdatedNotification(Patient updatedPatient);
		void SendTherapyPlaceTypeAddedNotification(TherapyPlaceType newTherapyPlaceType);
		void SendTherapyPlaceTypeUpdatedNotification(TherapyPlaceType updatedTherapyPlaceType);
		void SendLabelAddedNotification(Label newLabel);
		void SendLabelUpdatedNotification(Label updatedLabel);
	}
}
