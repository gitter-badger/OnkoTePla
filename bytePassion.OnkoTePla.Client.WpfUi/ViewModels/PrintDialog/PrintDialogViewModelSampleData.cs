using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PrintDialog
{
	internal class PrintDialogViewModelSampleData : IPrintDialogViewModel
	{
		public ICommand Cancel => null;
		public ICommand Print  => null;

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}