using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetPatientListRequest : NetworkMessageBase
	{
		public GetPatientListRequest(bool loadOnlyAlivePatients, ConnectionSessionId sessionId, Guid userId)
			: base (NetworkMessageType.GetPatientListRequest)
		{
			LoadOnlyAlivePatients = loadOnlyAlivePatients;
			SessionId = sessionId;
			UserId = userId;
		}

		public bool                LoadOnlyAlivePatients { get; }
		public ConnectionSessionId SessionId             { get; }
		public Guid                UserId                { get; }

		public override string AsString()
		{
			return $"{SessionId};{UserId};{LoadOnlyAlivePatients}";
		}

		public static GetPatientListRequest Parse(string s)
		{
			var parts = s.Split(';');

			var sessionId = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId    = Guid.Parse(parts[1]);
			var onlyAlive = bool.Parse(parts[2]);

			return new GetPatientListRequest(onlyAlive, sessionId, userId);
		}
	}
}
