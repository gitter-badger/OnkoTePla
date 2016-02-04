using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetTherapyPlacesTypeListRequest : NetworkMessageBase
	{
		public GetTherapyPlacesTypeListRequest (ConnectionSessionId sessionId, Guid userId)
			: base (NetworkMessageType.GetTherapyPlacesTypeListRequest)
		{			
			SessionId = sessionId;
			UserId = userId;			
		}
		 
		public ConnectionSessionId SessionId { get; }
		public Guid                UserId    { get; }		

		public override string AsString()
		{
			return $"{SessionId};{UserId}";
		}

		public static GetTherapyPlacesTypeListRequest Parse (string s)
		{
			var parts = s.Split(';');

			var sessionId              = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId                 =                         Guid.Parse(parts[1]);			
			
			return new GetTherapyPlacesTypeListRequest(sessionId, userId);
		}
	}
}
