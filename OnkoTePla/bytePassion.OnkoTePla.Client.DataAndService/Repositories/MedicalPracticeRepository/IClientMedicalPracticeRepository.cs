using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository
{
	public interface IClientMedicalPracticeRepository
	{
		void RequestMedicalPractice (Action<ClientMedicalPracticeData> practiceAvailableCallback,
									Guid practiceId, Action<string> errorCallback);

		void RequestMedicalPractice(Action<ClientMedicalPracticeData> practiceAvailableCallback, 
									Guid practiceId, uint version, Action<string> errorCallback);
		
		void RequestMedicalPractice(Action<ClientMedicalPracticeData> practiceAvailableCallback,
									Guid practiceId, Date day, Action<string> errorCallback);

		void RequestPraticeVersion(Action<uint> practiceVersionAvailableCallback, 
								   Guid practiceId, Date day, Action<string> errorCallback);
	}
}
