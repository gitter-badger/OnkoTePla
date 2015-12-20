using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ChangeConfirmationView
{
	public interface IChangeConfirmationViewModel : IViewModel
	{
		ICommand ConfirmChanges { get; }
		ICommand RejectChanges  { get; }
	}
}
