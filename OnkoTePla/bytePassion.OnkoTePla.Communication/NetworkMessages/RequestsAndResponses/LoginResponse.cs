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
			return "";
		}
		
		public static LoginResponse Parse (string s)
		{						
			return new LoginResponse();
		}
	}
}
