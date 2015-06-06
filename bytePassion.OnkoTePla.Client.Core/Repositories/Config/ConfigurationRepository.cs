using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Config
{
	public class ConfigurationRepository : IConfigurationRepository, IPersistablility
	{
		private Configuration configuration;
		private readonly IPersistenceService<Configuration> persistenceService; 

		public ConfigurationRepository(IPersistenceService<Configuration> persistenceService)
		{
			this.persistenceService = persistenceService;
		}

		public void SetConfig(Configuration newConfig)
		{
			configuration = newConfig;
		}

		public MedicalPractice  GetMedicalPracticeByName (string name) { return configuration.GetMedicalPracticeByName(name);  }
		public TherapyPlaceType GetTherapyPlaceTypeByName(string name) { return configuration.GetTherapyPlaceTypeByName(name); }

		public IEnumerable<MedicalPractice>  GetAllMedicalPractices()  { return configuration.GetAllMedicalPractices();  }
		public IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes() { return configuration.GetAllTherapyPlaceTypes(); }

		public void PersistRepository()
		{
			persistenceService.Persist(configuration);
		}

		public void LoadRepository()
		{
			configuration = persistenceService.Load();
		}
	}
}
