using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Contracts.NetworkMessages.Heartbeat
{
	public class Request
	{
		public Request(ConnectionSessionId sessionId)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId { get; }

		public string AsString ()
		{
			return $"HeartbeatRequest;{SessionId}";
		}

		public static Request Parse (string s)
		{
			var parts = s.Split(';');

			if (parts.Length != 2)
				throw new ArgumentException($"{s} is not a HeartbeatRequest");

			if (parts[0] != "HeartbeatRequest")
				throw new ArgumentException($"{s} is not a HeartbeatRequest");

			var sessionId = new ConnectionSessionId(Guid.Parse(parts[1]));			

			return new Request(sessionId);
		}
	}
}
