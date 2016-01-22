using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class HeartbeatRequest : NetworkMessageBase
	{		
		public HeartbeatRequest (ConnectionSessionId sessionId)
			: base(NetworkMessageType.HeartbeatRequest)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId { get; }

		public override string AsString ()
		{
			return $"{SessionId}";
        }

		public static HeartbeatRequest Parse (string s)
		{									
			return new HeartbeatRequest(new ConnectionSessionId(Guid.Parse(s)));
		}
	}
}
