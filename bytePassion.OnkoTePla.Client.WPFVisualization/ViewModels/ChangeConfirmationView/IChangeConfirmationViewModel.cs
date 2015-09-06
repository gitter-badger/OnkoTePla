using System.Windows.Input;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView
{
	public interface IChangeConfirmationViewModel
	{
		ICommand ConfirmChanges { get; }
		ICommand RejectChanges  { get; }
	}
}
