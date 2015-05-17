using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Contracts.Communication
{
	public interface IMedicalPracticeInfoProvider
	{
		MedicalPractice GetMedicalPractice();
	}
}
