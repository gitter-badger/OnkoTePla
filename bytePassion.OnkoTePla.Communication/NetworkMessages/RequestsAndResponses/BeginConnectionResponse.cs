using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class BeginConnectionResponse : NetworkMessageBase
	{		
		public BeginConnectionResponse(ConnectionSessionId sessionId)
			: base(NetworkMessageType.BeginConnectionResponse)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId { get; }

		public override string AsString()
		{
			return $"{SessionId}";
        }

		public static BeginConnectionResponse Parse(string s)
		{						
			return new BeginConnectionResponse(new ConnectionSessionId(Guid.Parse(s)));
		}
	}
}
