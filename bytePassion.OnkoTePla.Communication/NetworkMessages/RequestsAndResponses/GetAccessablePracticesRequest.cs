using System;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetAccessablePracticesRequest : NetworkMessageBase
	{
		public GetAccessablePracticesRequest (ConnectionSessionId sessionId, Guid userId) 
			: base(NetworkMessageType.GetAccessablePracticesRequest)
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
		 
		public static GetAccessablePracticesRequest Parse (string s)
		{
			var parts = s.Split(';').ToList();

			var sessionId = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId    = Guid.Parse(parts[1]);			
			
			return new GetAccessablePracticesRequest(sessionId, userId);
		}
	}
}
