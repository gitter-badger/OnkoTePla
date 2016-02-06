using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	internal class ConnectionInfo
	{
		public ConnectionInfo(ConnectionSessionId sessionId, ClientUserData loggedInUser)
		{
			SessionId = sessionId;
			LoggedInUser = loggedInUser;
		}
		
		public ConnectionSessionId SessionId    { get; }
		public ClientUserData      LoggedInUser { get; }
	}
}