using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Config
{
	public class ConfigurationRepository : IConfigurationRepository
	{
		private Configuration configuration;
		private readonly IPersistenceService<Configuration> persistenceService; 

		public ConfigurationRepository(IPersistenceService<Configuration> persistenceService, Configuration initialConfig=null)
		{
			this.persistenceService = persistenceService;

			if (initialConfig != null)
				configuration = initialConfig;
		}

		public uint GetLatestVersionFor(Guid medicalPractiveId)
		{
			var practice = GetMedicalPracticeById(medicalPractiveId);

			if (practice == null)
				throw new ArgumentException("there is no medicalPractice with that id");

			return practice.Version;
		}

		#region Wrapper around Configuration
		public MedicalPractice GetMedicalPracticeByName(string name)
		{
			return configuration.GetMedicalPracticeByName(name);
		}

		public MedicalPractice GetMedicalPracticeById(Guid id)
		{
			return configuration.GetMedicalPracticeById(id);
		}

		public MedicalPractice GetMedicalPracticeByIdAndVersion(Guid id, uint version)
		{
			return configuration.GetMedicalPracticeByIdAndVersion(id, version);
		}

		public void RemoveMedicalPractice(Guid medicalPracticeId)
		{
			configuration.RemoveMedicalPractice(medicalPracticeId);
		}

		public TherapyPlaceType GetTherapyPlaceTypeByName(string name)
		{
			return configuration.GetTherapyPlaceTypeByName(name);
		}

		public TherapyPlaceType GetTherapyPlaceTypeById(Guid id)
		{
			return configuration.GetTherapyPlaceTypeById(id);
		}

		public IEnumerable<MedicalPractice> GetAllMedicalPractices()
		{
			return configuration.GetAllMedicalPractices();
		}

		public void AddMedicalPractice(MedicalPractice practice)
		{
			configuration.AddMedicalPractice(practice);
		}

		public IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes()
		{
			return configuration.GetAllTherapyPlaceTypes();
		}

		public void AddTherapyPlaceType(TherapyPlaceType newTherapyPlaceType)
		{
			configuration.AddTherapyPlaceType(newTherapyPlaceType);
		}

		public User GetUserByName(string name)
		{
			return configuration.GetUserByName(name);
		}

		public User GetUserById(Guid id)
		{
			return configuration.GetUserById(id);
		}

		public IEnumerable<User> GetAllUsers()
		{
			return configuration.GetAllUsers();
		}

		public void AddUser(User newUser)
		{
			configuration.AddUser(newUser);
		}

		public void RemoveUser(Guid userId)
		{
			configuration.RemoveUser(userId);
		}
		#endregion

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
