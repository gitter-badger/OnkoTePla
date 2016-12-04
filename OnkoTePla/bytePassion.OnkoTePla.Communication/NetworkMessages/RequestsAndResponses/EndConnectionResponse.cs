namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class EndConnectionResponse : NetworkMessageBase
	{		
		public EndConnectionResponse() 
			: base(NetworkMessageType.EndConnectionResponse)
		{
		}

		public override string AsString ()
		{
			return "";
		}

		public static EndConnectionResponse Parse (string s)
		{									
			return new EndConnectionResponse();
		}		
	}
}
