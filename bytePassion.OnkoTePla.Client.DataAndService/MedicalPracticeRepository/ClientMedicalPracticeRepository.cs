using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.MedicalPracticeRepository
{
	public class ClientMedicalPracticeRepository : IClientMedicalPracticeRepository
	{
		public event Action<ClientMedicalPracticeData> MedicalPracticeAvailable;

		private readonly IConnectionService connectionService;
		private readonly IList<ClientMedicalPracticeData> cachedMedicalPractices;  

		internal ClientMedicalPracticeRepository(IConnectionService connectionService)
		{
			this.connectionService = connectionService;

			cachedMedicalPractices = new List<ClientMedicalPracticeData>();
		}

		public ClientMedicalPracticeData GetMedicalPractice(Guid id, uint version)
		{
			lock (cachedMedicalPractices)
			{
				if (IsMedicalPracticeAvailable(id, version))
					return cachedMedicalPractices.First(practice => practice.Id == id && practice.Version == version);
			}

			return null;
		}
		
		public bool IsMedicalPracticeAvailable(Guid id, uint version)
		{
			lock (cachedMedicalPractices)
			{
				return cachedMedicalPractices.Any(practice => practice.Id == id && practice.Version == version);
			}
		}

		public void RequestMedicalPractice(Action<ClientMedicalPracticeData> practiceAvailableCallback, Guid id, uint version, Action<string> errorCallback)
		{
			connectionService.RequestMedicalPractice(
				medicalPractice =>
				{
					lock (cachedMedicalPractices)
					{
						if (!IsMedicalPracticeAvailable(id, version))
							cachedMedicalPractices.Add(medicalPractice);
					}

					practiceAvailableCallback(medicalPractice);
				},
				id, version,
				errorCallback
			);
		}		
	}
}