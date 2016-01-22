using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class BeginDebugConnectionResponse : NetworkMessageBase
	{		
		public BeginDebugConnectionResponse (ConnectionSessionId sessionId)
			: base(NetworkMessageType.BeginDebugConnectionResponse)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId { get; }

		public override string AsString()
		{
			return $"{SessionId}";
        }

		public static BeginDebugConnectionResponse Parse (string s)
		{						
			return new BeginDebugConnectionResponse(new ConnectionSessionId(Guid.Parse(s)));
		}
	}
}
