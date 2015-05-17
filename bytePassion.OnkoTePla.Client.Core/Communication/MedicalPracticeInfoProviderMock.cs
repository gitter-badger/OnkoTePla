using bytePassion.OnkoTePla.Contracts;
using bytePassion.OnkoTePla.Contracts.Communication;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.Core.Communication
{
	public class MedicalPracticeInfoProviderMock : IMedicalPracticeInfoProvider
	{
		
		public MedicalPractice GetMedicalPractice()
		{
			return CommunicationSampleData.MedicalPractice;
		}
	}
}
