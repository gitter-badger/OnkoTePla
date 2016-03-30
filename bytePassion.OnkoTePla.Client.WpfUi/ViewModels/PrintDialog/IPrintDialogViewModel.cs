using System.Windows.Input;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PrintDialog
{
	internal interface IPrintDialogViewModel : IViewModel
	{
		ICommand Cancel { get; }
		ICommand Print  { get; }
	}
}