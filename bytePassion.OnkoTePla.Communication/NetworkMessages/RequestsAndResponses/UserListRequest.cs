using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class UserListRequest : NetworkMessageBase
	{
		public UserListRequest(ConnectionSessionId sessionId) 
			: base(NetworkMessageType.GetUserListRequest)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId;

		public override string AsString()
		{
			return SessionId.ToString();
		}

		public static UserListRequest Parse(string s)
		{
			Guid id;

			if (!Guid.TryParse(s, out id))
				throw new ArgumentException($"{s} ist noch a valid sessionId");

			return new UserListRequest(new ConnectionSessionId(id));
		}
	}
}
