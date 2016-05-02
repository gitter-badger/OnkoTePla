using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage
{
	internal interface IBackupPageViewModel : IViewModel
	{
		ICommand ImportData { get; }
		ICommand ExportData { get; }

		ObservableCollection<BackupInterval> AllBackupIntervals { get; }
		BackupInterval SelectedBackupInterval { get; set; }

		string BackupdestinationFolder { get; set; }
		ICommand SelectBackupFolder { get; }

		string BackupTime { get; set; }

		ObservableCollection<DayOfWeek> AllDaysOfWeek { get; }
		DayOfWeek SelectedDayOfWeek { get; set; }

		string BackupDay { get; set; }

		bool IsBackupFolderVisible    { get; }
		bool IsBackupTimeVisible      { get; }
		bool IsBackupDayOfWeekVisible { get; }
		bool IsBackupDayVisible       { get; }
	}
}