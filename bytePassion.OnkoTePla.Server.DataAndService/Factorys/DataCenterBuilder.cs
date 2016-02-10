using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Server.DataAndService.Data;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class DataCenterBuilder : IDataCenterBuilder
	{
		
		private readonly PatientRepository patientRepository;
		private readonly ConfigurationRepository configRepository;
		private readonly IEventStore eventStore;

		public DataCenterBuilder(PatientRepository patientRepository, 
								 ConfigurationRepository configRepository,
								 IEventStore eventStore)
		{
			this.patientRepository = patientRepository;
			this.configRepository = configRepository;
			this.eventStore = eventStore;
		}

		public IDataCenter Build()
		{
			return new DataCenter(configRepository, configRepository,
								  patientRepository, patientRepository,
								  eventStore);
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
