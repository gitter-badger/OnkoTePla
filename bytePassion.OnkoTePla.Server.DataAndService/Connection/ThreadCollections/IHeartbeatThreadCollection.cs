using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ThreadCollections
{
	internal interface IHeartbeatThreadCollection : IDisposable
	{
		event Action<ConnectionSessionId> ClientVanished;

		void StopThread(ConnectionSessionId sessionId);
		void AddThread(AddressIdentifier clientAddressIdentifier, ConnectionSessionId id);
	}
}