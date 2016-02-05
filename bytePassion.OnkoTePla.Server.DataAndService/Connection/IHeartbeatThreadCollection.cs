using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	internal interface IHeartbeatThreadCollection : IDisposable
	{
		void StopThread(ConnectionSessionId sessionId);
		void AddThread(AddressIdentifier clientAddressIdentifier, ConnectionSessionId id);
	}
}