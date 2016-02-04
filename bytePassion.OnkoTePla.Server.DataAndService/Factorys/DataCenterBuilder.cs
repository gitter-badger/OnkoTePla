using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Server.DataAndService.Data;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class DataCenterBuilder : IDataCenterBuilder
	{
		
		private readonly PatientRepository patientRepository;
		private readonly ConfigurationRepository configRepository;

		public DataCenterBuilder()
		{			
			
		}

		public IDataCenter Build()
		{
			return new DataCenter(configRepository, configRepository,
								  patientRepository, patientRepository);
		}

		public void PersistConfig()
		{
			configRepository.PersistRepository();
		}
	}
}
