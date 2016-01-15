using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public class SessionInfo
	{
		public SessionInfo(ConnectionSessionId sessionId, Time creationTime, 
						   AddressIdentifier clientAddress, bool isDebugConnection, 
						   User loggedInUser = null)
		{
			SessionId = sessionId;
			CreationTime = creationTime;
			ClientAddress = clientAddress;
			IsDebugConnection = isDebugConnection;
			LoggedInUser = loggedInUser;
		}

		public ConnectionSessionId SessionId         { get; }
		public Time                CreationTime      { get; }
		public AddressIdentifier   ClientAddress     { get; }
		public bool                IsDebugConnection { get; }
		public User                LoggedInUser      { get; }
	}
}