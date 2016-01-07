using bytePassion.Lib.Types.Repository;

namespace bytePassion.OnkoTePla.Client.DataAndService.LocalSettings
{
	internal class LocalSettingsRepository : ILocalSettingsRepository
	{
		private readonly IPersistenceService<LocalSettingsData> persistenceService;
				
		public LocalSettingsRepository(IPersistenceService<LocalSettingsData> persistenceService)
		{
			this.persistenceService = persistenceService;
		}

		public void PersistRepository()
		{
			persistenceService.Persist(new LocalSettingsData(IsAutoConnectionEnabled));
		}

		public void LoadRepository()
		{
			var newSettings = persistenceService.Load();
			IsAutoConnectionEnabled = newSettings.IsAutoConnectionEnabled;
		}

		public bool IsAutoConnectionEnabled { get; set; }
	}
}
