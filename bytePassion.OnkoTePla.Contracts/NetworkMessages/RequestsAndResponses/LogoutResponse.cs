using System;

namespace bytePassion.OnkoTePla.Contracts.NetworkMessages.RequestsAndResponses
{
	public class LogoutResponse : NetworkMessageBase
	{
		public LogoutResponse () 
			: base(NetworkMessageType.LogoutResponse)
		{			
		}		
		
		public override string AsString()
		{
			return nameof(LogoutResponse);
		}
		
		public static LogoutResponse Parse (string s)
		{
			if (s != nameof(LogoutResponse))
				throw new Exception();
			
			return new LogoutResponse();
		}
	}
}
