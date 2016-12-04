using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetLabelListRequest : NetworkMessageBase
	{
		public GetLabelListRequest (ConnectionSessionId sessionId) 
			: base(NetworkMessageType.GetLabelListRequest)
		{
			SessionId = sessionId;
		}
		
		public ConnectionSessionId SessionId;

		public override string AsString()
		{
			return SessionId.ToString();
		}

		public static GetLabelListRequest Parse (string s)
		{
			return new GetLabelListRequest(new ConnectionSessionId(Guid.Parse(s)));
		}
	}
}
