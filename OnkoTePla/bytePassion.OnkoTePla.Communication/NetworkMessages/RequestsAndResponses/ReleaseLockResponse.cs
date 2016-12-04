namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class ReleaseLockResponse : NetworkMessageBase
	{
		public ReleaseLockResponse ()
			: base(NetworkMessageType.ReleaseLockResponse)
		{
		}

		public override string AsString ()
		{
			return "";
		}
		
		public static ReleaseLockResponse Parse (string s)
		{
			return new ReleaseLockResponse();
		}
	}
}