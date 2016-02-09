﻿using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.Types.Repository;

namespace bytePassion.OnkoTePla.Client.DataAndService.LocalSettings
{
	public class LocalSettingsRepository : ILocalSettingsRepository
	{
		private readonly IPersistenceService<LocalSettingsData> persistenceService;
				
		public LocalSettingsRepository(IPersistenceService<LocalSettingsData> persistenceService)
		{
			this.persistenceService = persistenceService;
		}

		public void PersistRepository()
		{
			persistenceService.Persist(new LocalSettingsData(IsAutoConnectionEnabled, 
															 AutoConnectionClientAddress,
															 AutoConnectionServerAddress, 
															 LastUsedMedicalPracticeId));
		}

		public void LoadRepository()
		{
			var newSettings = persistenceService.Load();

			IsAutoConnectionEnabled     = newSettings.IsAutoConnectionEnabled;
			AutoConnectionClientAddress = newSettings.AutoConnectionClientAddress;
			AutoConnectionServerAddress = newSettings.AutoConnectionServerAddress;
			LastUsedMedicalPracticeId   = newSettings.LastUsedMedicalPracticeId;
		}

		public bool              IsAutoConnectionEnabled     { get; set; }
		public AddressIdentifier AutoConnectionClientAddress { get; set; }
		public AddressIdentifier AutoConnectionServerAddress { get; set; }
		public Guid              LastUsedMedicalPracticeId   { get; set; }
	}
}
