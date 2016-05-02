using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.LocalSettings
{
	public class LocalSettingsData
	{
		public static LocalSettingsData CreateDefaultSettings()
		{
			return new LocalSettingsData(BackupInterval.None, string.Empty,
										 Time.Dummy, DayOfWeek.Monday, -1,
										 DateTime.MinValue);
		}

		public LocalSettingsData(BackupInterval backupInterval, string backupDirectory,
								 Time backupTime, DayOfWeek backupWeekDay, int backUpDay, 
								 DateTime lastBackup)
		{
			BackupInterval  = backupInterval;
			BackupDirectory = backupDirectory;
			BackupTime      = backupTime;
			BackupWeekDay   = backupWeekDay;
			BackUpDay       = backUpDay;			
			LastBackup      = lastBackup;			
		}
		
		public BackupInterval BackupInterval  { get; }
		public string         BackupDirectory { get; }
		public Time           BackupTime      { get; }
		public DayOfWeek      BackupWeekDay   { get; }
		public int            BackUpDay       { get; }		
		public DateTime       LastBackup      { get; }
	}
}