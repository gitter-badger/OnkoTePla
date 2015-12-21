using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ChangeConfirmationView
{
	public class ChangeConfirmationViewModelSampleData : IChangeConfirmationViewModel
	{
		public ICommand ConfirmChanges { get; } = null;
		public ICommand RejectChanges  { get; } = null;
	    
	    public void Dispose() { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
