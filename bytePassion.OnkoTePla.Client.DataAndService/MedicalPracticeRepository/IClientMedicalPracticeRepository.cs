using System;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.MedicalPracticeRepository
{
	public interface IClientMedicalPracticeRepository
	{		
		ClientMedicalPracticeData GetMedicalPractice(Guid id, uint version);
		bool IsMedicalPracticeAvailable(Guid id, uint version);
		void RequestMedicalPractice(Action<ClientMedicalPracticeData> practiceAvailableCallback, 
									Guid id, uint version, Action<string> errorCallback);
	}
}
