using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetPatientListRequest : NetworkMessageBase
	{
		public GetPatientListRequest(ConnectionSessionId sessionId, Guid userId)
			: base (NetworkMessageType.GetPatientListRequest)
		{			
			SessionId = sessionId;
			UserId = userId;
		}
		
		public ConnectionSessionId SessionId             { get; }
		public Guid                UserId                { get; }

		public override string AsString()
		{
			return $"{SessionId};{UserId}";
		}

		public static GetPatientListRequest Parse(string s)
		{
			var parts = s.Split(';');

			var sessionId = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId    = Guid.Parse(parts[1]);			

			return new GetPatientListRequest(sessionId, userId);
		}
	}
}
