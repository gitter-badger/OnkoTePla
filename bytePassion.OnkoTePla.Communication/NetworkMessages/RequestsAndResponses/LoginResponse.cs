using System;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class LoginResponse : NetworkMessageBase
	{
		public LoginResponse () 
			: base(NetworkMessageType.LoginResponse)
		{			
		}		

		public override string AsString()
		{
			return nameof(LoginResponse);
		}
		
		public static LoginResponse Parse (string s)
		{
			if (s != nameof(LoginResponse))
				throw new Exception();
			
			return new LoginResponse();
		}
	}
}
