﻿using System;
using System.Collections.Generic;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.Config
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

		public void UpdateTherapyPlaceType(TherapyPlaceType updatedTherapyPlaceType)
		{
			configuration.UpdateTherapyPlaceTupe(updatedTherapyPlaceType);
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

	    public void UpdateUser(User updatedUser)
	    {
		    configuration.UpdateUser(updatedUser);
	    }

		public Label GetLabelById(Guid id)
		{
			return configuration.GetLabelById(id);
		}

		public IEnumerable<Label> GetAllLabels()
		{
			return configuration.GetAllLabels();
		}

		public void AddLabel(Label newLabel)
		{
			configuration.AddLabel(newLabel);
		}
		
		public void UpdateLabel(Label updatedLabel)
		{
			configuration.UpdateLabel(updatedLabel);
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
