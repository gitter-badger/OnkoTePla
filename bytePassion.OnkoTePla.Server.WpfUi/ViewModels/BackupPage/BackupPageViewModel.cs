using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage
{
	internal class BackupPageViewModel : ViewModel, IBackupPageViewModel
	{
		private static readonly string TestFile = GlobalConstants.BackupBasePath + "test.zip";
		
		public BackupPageViewModel(IBackupService backupService)
		{
			ImportData = new Command(() => backupService.Import(TestFile));
			ExportData = new Command(() => backupService.Export(TestFile));
		}
					
		public ICommand ImportData { get; }
		public ICommand ExportData { get; }

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
