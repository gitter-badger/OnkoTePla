using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public interface IConnectionService : IDisposable
	{
		event Action<ConnectionSessionId> NewSessionStarted;
		event Action<ConnectionSessionId> SessionTerminated;

		void InitiateCommunication(Address serverAddress);
		void StopCommunication();

		AddressIdentifier GetAddress(ConnectionSessionId sessionId);
		TimeSpan GetSessionStartTime(ConnectionSessionId sessionId);
	}
}
