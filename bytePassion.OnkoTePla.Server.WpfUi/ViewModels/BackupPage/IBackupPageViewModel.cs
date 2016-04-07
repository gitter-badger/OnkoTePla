using System.Windows.Input;
using bytePassion.Lib.WpfLib.ViewModelBase;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage
{
	internal interface IBackupPageViewModel : IViewModel
	{
		ICommand ImportData { get; }
		ICommand ExportData { get; }
	}
}