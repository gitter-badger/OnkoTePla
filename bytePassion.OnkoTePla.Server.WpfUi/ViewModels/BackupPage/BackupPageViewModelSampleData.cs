using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage
{
	internal class BackupPageViewModelSampleData : IBackupPageViewModel
	{
		public BackupPageViewModelSampleData()
		{
			AllBackupIntervals = typeof(BackupInterval).GetProperties(BindingFlags.Static | BindingFlags.Public)
													   .Select(p => (BackupInterval)p.GetValue(null, null))											
													   .ToObservableCollection();

			AllDaysOfWeek = typeof(DayOfWeek).GetProperties(BindingFlags.Static | BindingFlags.Public)
													   .Select(p => (DayOfWeek)p.GetValue(null, null))
													   .ToObservableCollection();

			SelectedBackupInterval = AllBackupIntervals[2];
			BackupdestinationFolder = @"C:\test";
			BackupTime = "18:30";			
			SelectedDayOfWeek = AllDaysOfWeek[4];
			BackupDay = "10";

			IsBackupDayOfWeekVisible = true;
			IsBackupDayVisible       = true;
			IsBackupFolderVisible    = true;
			IsBackupTimeVisible      = true;
		}

		public ICommand ImportData         => null;
		public ICommand ExportData         => null;
		public ICommand SelectBackupFolder => null;

		public ObservableCollection<BackupInterval> AllBackupIntervals { get; }
		public ObservableCollection<DayOfWeek> AllDaysOfWeek { get; }

		public BackupInterval SelectedBackupInterval  { get; set; }
		public string         BackupdestinationFolder { get; set; }		
		public string         BackupTime              { get; set; }		
		public DayOfWeek      SelectedDayOfWeek       { get; set; }
		public string         BackupDay               { get; set; }

		public bool IsBackupFolderVisible    { get; }
		public bool IsBackupTimeVisible      { get; }
		public bool IsBackupDayOfWeekVisible { get; }
		public bool IsBackupDayVisible       { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}