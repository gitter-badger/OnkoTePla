using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Contracts.NetworkMessages.Heartbeat
{
	public class Response
	{
		private const string MsgIdentifier = "HearbeatConfirmed";

		public Response(ConnectionSessionId sessionId)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId { get; }

		public string AsString ()
		{
			return $"{MsgIdentifier};{SessionId}";
        }

		public static Response Parse (string s)
		{
			var parts = s.Split(';');

			if (parts.Length != 2)
				throw new ArgumentException($"{s} is not a {MsgIdentifier}");
			
			if (parts[0] != MsgIdentifier)
				throw new ArgumentException($"{s} is not a {MsgIdentifier}");

			var sessionId = new ConnectionSessionId(Guid.Parse(parts[1]));
			return new Response(sessionId);
		}
	}
}
