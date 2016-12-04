using System;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.LocalSettings
{
	public class LocalSettingsRepository : ILocalSettingsRepository
	{
		private readonly IPersistenceService<LocalSettingsData> persistenceService;
				
		public LocalSettingsRepository(IPersistenceService<LocalSettingsData> persistenceService)
		{
			this.persistenceService = persistenceService;
			ApplySettings(LocalSettingsData.CreateDefaultSettings());
		}

		public void PersistRepository()
		{
			persistenceService.Persist(new LocalSettingsData(BackupInterval, BackupDirectory,
															 BackupTime, BackupWeekDay, BackUpDay,
															 LastBackup));
		}

		public void LoadRepository()
		{
			var newSettings = persistenceService.Load();
			ApplySettings(newSettings);			
		}

		private void ApplySettings(LocalSettingsData newSettings)
		{
			BackupInterval  = newSettings.BackupInterval;
			BackupDirectory = newSettings.BackupDirectory;
			BackupTime      = newSettings.BackupTime;
			BackupWeekDay   = newSettings.BackupWeekDay;
			BackUpDay       = newSettings.BackUpDay;			
			LastBackup      = newSettings.LastBackup;	
		}
		
		public BackupInterval BackupInterval  { get; set; }
		public string         BackupDirectory { get; set; }
		public Time           BackupTime      { get; set; }
		public DayOfWeek      BackupWeekDay   { get; set; }
		public int            BackUpDay       { get; set; }		
		public DateTime       LastBackup      { get; set; }
	}
}
