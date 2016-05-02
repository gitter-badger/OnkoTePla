using System;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.LocalSettings
{
	public interface ILocalSettingsRepository : IPersistable
	{
		BackupInterval BackupInterval  { get; set; }
		string         BackupDirectory { get; set; }		
		Time           BackupTime      { get; set; }
		DayOfWeek      BackupWeekDay   { get; set; }
		int            BackUpDay       { get; set; }		
		DateTime       LastBackup      { get; set; }
	}
}