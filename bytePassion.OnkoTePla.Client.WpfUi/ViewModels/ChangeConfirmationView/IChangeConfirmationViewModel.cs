using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ChangeConfirmationView
{
    public interface IChangeConfirmationViewModel
	{
		ICommand ConfirmChanges { get; }
		ICommand RejectChanges  { get; }
	}
}
