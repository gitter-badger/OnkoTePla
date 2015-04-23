using xIT.OnkoTePla.Contracts;
using xIT.OnkoTePla.Contracts.Communication;
using xIT.OnkoTePla.Contracts.Infrastructure;


namespace xIT.OnkoTePla.Client.Core.Communication
{
	public class MedicalPracticeInfoProviderMock : IMedicalPracticeInfoProvider
	{
		
		public MedicalPractice GetMedicalPractice()
		{
			return CommunicationSampleData.MedicalPractice;
		}
	}
}
