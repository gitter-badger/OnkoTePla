using bytePassion.Lib.Types.Communication;
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
			persistenceService.Persist(new LocalSettingsData(IsAutoConnectionEnabled, 
															 AutoConnectionAddress));
		}

		public void LoadRepository()
		{
			var newSettings = persistenceService.Load();

			IsAutoConnectionEnabled = newSettings.IsAutoConnectionEnabled;
			AutoConnectionAddress   = newSettings.AutoConnectionAddress;
		}

		public bool              IsAutoConnectionEnabled { get; set; }
		public AddressIdentifier AutoConnectionAddress   { get; set; }
	}
}
