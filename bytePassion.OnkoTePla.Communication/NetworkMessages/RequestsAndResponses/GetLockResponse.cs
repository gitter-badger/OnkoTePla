namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetLockResponse : NetworkMessageBase
	{
		public GetLockResponse()
			: base(NetworkMessageType.GetLockResponse)
		{			
		}

		public override string AsString()
		{
			return "";
		}

		public static GetLockResponse Parse(string s)
		{
			return new GetLockResponse();
		}
	}
}