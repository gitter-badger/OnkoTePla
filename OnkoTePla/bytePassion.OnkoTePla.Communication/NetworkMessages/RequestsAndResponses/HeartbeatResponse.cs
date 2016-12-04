using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class HeartbeatResponse : NetworkMessageBase
	{		
		public HeartbeatResponse (ConnectionSessionId sessionId)
			: base(NetworkMessageType.HeartbeatResponse)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId { get; }

		public override string AsString ()
		{
			return $"{SessionId}";
        }
		
		public static HeartbeatResponse Parse (string s)
		{									
			return new HeartbeatResponse(new ConnectionSessionId(Guid.Parse(s)));
		}
	}
}
