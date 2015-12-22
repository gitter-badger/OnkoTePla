using System;

namespace bytePassion.OnkoTePla.Contracts.NetworkMessages.EndConnection
{
	public class Response
	{
		private const string MsgIdentifier = "ConnectionEndConfirmed";				

		public string AsString ()
		{
			return MsgIdentifier;
		}

		public static Response Parse (string s)
		{			
			if (s != MsgIdentifier)
				throw new ArgumentException($"{s} is not a {MsgIdentifier}");
			
			return new Response();
		}
	}
}
