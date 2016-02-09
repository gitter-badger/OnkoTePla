namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetPracticeVersionInfoResponse : NetworkMessageBase
	{
		public GetPracticeVersionInfoResponse (uint medicalPracticeVersion)
			: base(NetworkMessageType.GetPracticeVersionInfoResponse)
		{
			MedicalPracticeVersion = medicalPracticeVersion;			
		}
		
		public uint MedicalPracticeVersion { get; } 

		public override string AsString()
		{
			return MedicalPracticeVersion.ToString();
		}

		public static GetPracticeVersionInfoResponse Parse (string s)
		{			
			return new GetPracticeVersionInfoResponse(uint.Parse(s));
		}
	}
}