using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage
{
	internal class BackupPageViewModelSampleData : IBackupPageViewModel
	{
		public ICommand ImportData => null;
		public ICommand ExportData => null;

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}