using bytePassion.OnkoTePla.Server.DataAndService.Backup;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.EventStore;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.Patients;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class DataCenterBuilder : IDataCenterBuilder
	{
		
		private readonly IPatientRepository patientRepository;
		private readonly IConfigurationRepository configRepository;
		private readonly IEventStore eventStore;

		public DataCenterBuilder(IPatientRepository patientRepository, 
								 IConfigurationRepository configRepository,
								 IEventStore eventStore)
		{
			this.patientRepository = patientRepository;
			this.configRepository = configRepository;
			this.eventStore = eventStore;
		}
		
		public IDataCenter Build()
		{
			return new DataCenter(configRepository,
								  patientRepository,
								  eventStore, 
								  new BackupService());
		}

		public void PersistConfigRepostiory()
		{
			configRepository.PersistRepository();
		}

		public void PersistPatientRepository()
		{
			patientRepository.PersistRepository();
		}

		public void PersistEventStore()
		{
			eventStore.PersistRepository();
		}
	}
}
