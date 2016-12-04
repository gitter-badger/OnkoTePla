namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetLockResponse : NetworkMessageBase
	{
		public GetLockResponse(bool successful)
			: base(NetworkMessageType.GetLockResponse)
		{
			Successful = successful;
		}

		public override string AsString()
		{
			return Successful.ToString();
		}

		public bool Successful { get; }

		public static GetLockResponse Parse(string s)
		{
			return new GetLockResponse(bool.Parse(s));
		}
	}
}