using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.EventStore;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.Patients;

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
