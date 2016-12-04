using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class EndConnectionRequest : NetworkMessageBase
	{		
		public EndConnectionRequest (ConnectionSessionId sessionId)
			: base (NetworkMessageType.EndConnectionRequest)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId { get; }

		public override string AsString ()
		{
			return $"{SessionId}";
        }

		public static EndConnectionRequest Parse (string s)
		{						
			return new EndConnectionRequest(new ConnectionSessionId(Guid.Parse(s)));
		}
	}
}
