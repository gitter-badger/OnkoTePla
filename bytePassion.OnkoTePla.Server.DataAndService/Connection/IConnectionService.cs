using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public interface IConnectionService : IDisposable
	{
		event Action<SessionInfo> NewSessionStarted;
		event Action<SessionInfo> SessionTerminated;
		event Action<SessionInfo> LoggedInUserUpdated;
		
		SessionInfo GetSessionInfo(ConnectionSessionId id);


		void InitiateCommunication (Address serverAddress);
		void StopCommunication ();
	}
}
