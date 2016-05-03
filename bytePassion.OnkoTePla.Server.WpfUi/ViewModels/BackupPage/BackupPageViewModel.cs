using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.LocalSettings;
using Microsoft.Win32;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage
{
	internal class BackupPageViewModel : ViewModel, IBackupPageViewModel
	{
		private readonly IBackupService backupService;
		private readonly ILocalSettingsRepository localSettingsRepository;

		private BackupInterval selectedBackupInterval;
		private string backupdestinationFolder;
		private string backupTime;
		private DayOfWeek selectedDayOfWeek;
		private string backupDay;
		private bool isBackupFolderVisible;
		private bool isBackupTimeVisible;
		private bool isBackupDayOfWeekVisible;
		private bool isBackupDayVisible;
		private bool isActivateButtonVisible;
		private bool isDeactivateButtonVisible;

		public BackupPageViewModel(IBackupService backupService,
								   ILocalSettingsRepository localSettingsRepository)
		{
			this.backupService = backupService;
			this.localSettingsRepository = localSettingsRepository;

			AllBackupIntervals = typeof(BackupInterval).GetProperties(BindingFlags.Static | BindingFlags.Public)
													   .Select(p => (BackupInterval)p.GetValue(null, null))
													   .ToObservableCollection();

			AllDaysOfWeek = typeof(DayOfWeek).GetProperties(BindingFlags.Static | BindingFlags.Public)
													   .Select(p => (DayOfWeek)p.GetValue(null, null))
													   .ToObservableCollection();

			ImportData               = new Command(DoImportData);
			ExportData               = new Command(DoExportData);
			SelectBackupFolder       = new Command(DoSelectBackupFolder);
			ActivateBackupSchedule   = new Command(DoActivateBackupSchedule);
			DeactivateBackupSchedule = new Command(DoDeactivateBackupScheldule);
		}
		
		public ICommand ImportData               { get; }
		public ICommand ExportData               { get; }
		public ICommand SelectBackupFolder       { get; }
		public ICommand ActivateBackupSchedule   { get; }
		public ICommand DeactivateBackupSchedule { get; }

		public ObservableCollection<BackupInterval> AllBackupIntervals { get; }
		public ObservableCollection<DayOfWeek>      AllDaysOfWeek      { get; }

		public BackupInterval SelectedBackupInterval
		{
			get { return selectedBackupInterval; }
			set { PropertyChanged.ChangeAndNotify(this, ref selectedBackupInterval, value); }
		}

		public string BackupdestinationFolder
		{
			get { return backupdestinationFolder; }
			set { PropertyChanged.ChangeAndNotify(this, ref backupdestinationFolder, value); }
		}

		public string BackupTime
		{
			get { return backupTime; }
			set { PropertyChanged.ChangeAndNotify(this, ref backupTime, value); }
		}

		public DayOfWeek SelectedDayOfWeek
		{
			get { return selectedDayOfWeek; }
			set { PropertyChanged.ChangeAndNotify(this, ref selectedDayOfWeek, value); }
		}

		public string BackupDay
		{
			get { return backupDay; }
			set { PropertyChanged.ChangeAndNotify(this, ref backupDay, value); }
		}

		public bool IsBackupFolderVisible
		{
			get { return isBackupFolderVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isBackupFolderVisible, value); }
		}

		public bool IsBackupTimeVisible
		{
			get { return isBackupTimeVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isBackupTimeVisible, value); }
		}

		public bool IsBackupDayOfWeekVisible
		{
			get { return isBackupDayOfWeekVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isBackupDayOfWeekVisible, value); }
		}

		public bool IsBackupDayVisible
		{
			get { return isBackupDayVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isBackupDayVisible, value); }
		}

		public bool IsActivateButtonVisible
		{
			get { return isActivateButtonVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isActivateButtonVisible, value); }
		}

		public bool IsDeactivateButtonVisible
		{
			get { return isDeactivateButtonVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isDeactivateButtonVisible, value); }
		}

		private void DoImportData()
		{			
			var openFileDialog = new OpenFileDialog
			{
				DefaultExt = ".obf",
				Filter = "OnkoTePla Backup Files (*.obf)|*.obf"
			};

			var dialogResult = openFileDialog.ShowDialog();			
			if (dialogResult == true)
			{								
				backupService.Import(openFileDialog.FileName);			
			}
		}

		private void DoExportData ()
		{
			var saveFileDialog = new SaveFileDialog
			{
				DefaultExt = ".obf",
				Filter = "OnkoTePla Backup Files (*.obf)|*.obf"
			};

			var dialogResult = saveFileDialog.ShowDialog();
			if (dialogResult == true)
			{
				backupService.Export(saveFileDialog.FileName);
			}			
		}

		private void DoSelectBackupFolder ()
		{
			// TODO
		}

		private void DoDeactivateBackupScheldule ()
		{
			// TODO
		}

		private void DoActivateBackupSchedule ()
		{
			// TODO
		}

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
