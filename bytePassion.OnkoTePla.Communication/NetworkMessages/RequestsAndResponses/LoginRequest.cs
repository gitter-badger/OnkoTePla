using System;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class LoginRequest : NetworkMessageBase
	{
		public LoginRequest (ConnectionSessionId sessionId, Guid userId, string passord) 
			: base(NetworkMessageType.LoginRequest)
		{
			SessionId = sessionId;
			UserId = userId;
			Password = passord;
		}

		public ConnectionSessionId SessionId { get; }
		public Guid                UserId    { get; }
		public string              Password  { get; }

		public override string AsString()
		{
			return $"{SessionId};{UserId};{Password}";
		}

		public static LoginRequest Parse (string s)
		{
			var parts = s.Split(';').ToList();

			var sessionId = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId    = Guid.Parse(parts[1]);
			var password  = parts[2];
			
			return new LoginRequest(sessionId, userId, password);
		}
	}
}
