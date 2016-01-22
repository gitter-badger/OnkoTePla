using System;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Connection;

namespace bytePassion.OnkoTePla.Server.DataAndService.SessionRepository
{
	internal interface ICurrentSessionsInfo
	{
		event Action<SessionInfo> NewSessionStarted;
		event Action<SessionInfo> SessionTerminated;
		event Action<SessionInfo> LoggedInUserUpdated;

		SessionInfo GetSessionInfo (ConnectionSessionId id);

		bool DoesSessionExist(ConnectionSessionId id);
		bool IsUserLoggedIn(Guid userId);
		bool IsClientAddressConnected(AddressIdentifier clientAddress);
		SessionInfo GetSessionForUser(Guid userId);
		
		void AddSession(ConnectionSessionId sessionId, Time creationTime,
						AddressIdentifier clientAddress, bool isDebugConnection);

		void RemoveSession(ConnectionSessionId sessionId);
		
		void UpdateLoggedInUser(ConnectionSessionId sessionId, User newUser);
	}
}