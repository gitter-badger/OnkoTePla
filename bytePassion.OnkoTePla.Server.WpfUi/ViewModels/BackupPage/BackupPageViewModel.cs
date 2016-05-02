using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.LocalSettings;
using Microsoft.Win32;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage
{
	internal class BackupPageViewModel : ViewModel, IBackupPageViewModel
	{
		private readonly IBackupService backupService;
		private readonly ILocalSettingsRepository localSettingsRepository;

		public BackupPageViewModel(IBackupService backupService,
								   ILocalSettingsRepository localSettingsRepository)
		{
			this.backupService = backupService;
			this.localSettingsRepository = localSettingsRepository;

			ImportData = new Command(DoImportData);
			ExportData = new Command(DoExportData);
		}		

		public ICommand ImportData { get; }
		public ICommand ExportData { get; }

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

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
