namespace bytePassion.OnkoTePla.Contracts.NetworkMessages.DataRequests
{
	public class ErrorResponse : NetworkMessageBase
	{
		public ErrorResponse (string errorMessage) 
			: base(NetworkMessageType.ErrorResponse)
		{
			ErrorMessage = errorMessage;
		}

		public string ErrorMessage { get; } 

		public override string AsString()
		{
			return ErrorMessage;			
		}

		public static ErrorResponse Parse (string s)
		{			
			return new ErrorResponse(s);
		}
	}
}
