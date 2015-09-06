using System.Windows.Input;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView
{
	public class ChangeConfirmationViewModelSampleData : IChangeConfirmationViewModel
	{
		public ICommand ConfirmChanges { get; } = null;
		public ICommand RejectChanges  { get; } = null;
	}
}
