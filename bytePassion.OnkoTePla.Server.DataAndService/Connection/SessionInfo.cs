using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	internal class SessionInfo
	{
		public SessionInfo(ConnectionSessionId sessionId, Time creationTime, 
						   AddressIdentifier clientAddress, bool isDebugConnection)
		{
			SessionId = sessionId;
			CreationTime = creationTime;
			ClientAddress = clientAddress;
			IsDebugConnection = isDebugConnection;
		}

		public ConnectionSessionId SessionId         { get; }
		public Time                CreationTime      { get; }
		public AddressIdentifier   ClientAddress     { get; }
		public bool                IsDebugConnection { get; }
	}
}