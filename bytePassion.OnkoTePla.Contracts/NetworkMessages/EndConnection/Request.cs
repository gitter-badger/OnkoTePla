using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Contracts.NetworkMessages.EndConnection
{
	public class Request
	{
		private const string MsgIdentifier = "EndConnectionRequest";

		public Request (ConnectionSessionId sessionId)
		{
			SessionId = sessionId;
		}

		public ConnectionSessionId SessionId { get; }

		public string AsString ()
		{
			return $"{MsgIdentifier};{SessionId}";
        }

		public static Request Parse (string s)
		{
			var parts = s.Split(';');

			if (parts.Length != 2)
				throw new ArgumentException($"{s} is not a {MsgIdentifier}");

			if (parts[0] != MsgIdentifier)
				throw new ArgumentException($"{s} is not a {MsgIdentifier}");

			var sessionId = new ConnectionSessionId(Guid.Parse(parts[1]));
			return new Request(sessionId);
		}
	}
}
