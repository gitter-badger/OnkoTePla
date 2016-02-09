using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Server.DataAndService.Data;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class DataCenterBuilder : IDataCenterBuilder
	{
		
		private readonly PatientRepository patientRepository;
		private readonly ConfigurationRepository configRepository;

		public DataCenterBuilder(PatientRepository patientRepository, 
								 ConfigurationRepository configRepository)
		{
			this.patientRepository = patientRepository;
			this.configRepository = configRepository;
		}

		public IDataCenter Build()
		{
			return new DataCenter(configRepository, configRepository,
								  patientRepository, patientRepository);
		}

		public void PersistConfigRepostiory()
		{
			configRepository.PersistRepository();
		}

		public void PersistPatientRepository()
		{
			patientRepository.PersistRepository();
		}			
	}
}
