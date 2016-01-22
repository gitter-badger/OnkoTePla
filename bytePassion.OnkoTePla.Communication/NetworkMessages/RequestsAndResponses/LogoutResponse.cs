namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class LogoutResponse : NetworkMessageBase
	{
		public LogoutResponse () 
			: base(NetworkMessageType.LogoutResponse)
		{			
		}		
		
		public override string AsString()
		{
			return "";
		}
		
		public static LogoutResponse Parse (string s)
		{			
			return new LogoutResponse();
		}
	}
}
