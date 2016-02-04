using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetTherapyPlacesTypeListResponse : NetworkMessageBase
	{
		public GetTherapyPlacesTypeListResponse (ClientMedicalPracticeData medicalPractice)
			: base(NetworkMessageType.GetTherapyPlacesTypeListResponse)
		{
			MedicalPractice = medicalPractice;			
		}

		public ClientMedicalPracticeData MedicalPractice { get; } 

		public override string AsString()
		{
			return ClientMedicalPracticeDataSerializer.Serialize(MedicalPractice);
		}

		public static GetTherapyPlacesTypeListResponse Parse (string s)
		{
			var medicalPractice = ClientMedicalPracticeDataSerializer.Deserialize(s);		
			return new GetTherapyPlacesTypeListResponse(medicalPractice);
		}
	}
}