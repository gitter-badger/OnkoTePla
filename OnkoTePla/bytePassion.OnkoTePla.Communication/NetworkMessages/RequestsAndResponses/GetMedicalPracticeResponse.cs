using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetMedicalPracticeResponse : NetworkMessageBase
	{
		public GetMedicalPracticeResponse (ClientMedicalPracticeData medicalPractice)
			: base(NetworkMessageType.GetMedicalPracticeResponse)
		{
			MedicalPractice = medicalPractice;			
		}

		public ClientMedicalPracticeData MedicalPractice { get; } 

		public override string AsString()
		{
			return ClientMedicalPracticeDataSerializer.Serialize(MedicalPractice);
		}

		public static GetMedicalPracticeResponse Parse (string s)
		{
			var medicalPractice = ClientMedicalPracticeDataSerializer.Deserialize(s);		
			return new GetMedicalPracticeResponse(medicalPractice);
		}
	}
}