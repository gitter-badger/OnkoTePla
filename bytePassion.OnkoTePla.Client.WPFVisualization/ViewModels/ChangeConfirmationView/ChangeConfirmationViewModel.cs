using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView
{
	public class ChangeConfirmationViewModel : IChangeConfirmationViewModel
	{
		public ChangeConfirmationViewModel(ViewModelCommunication<ViewModelMessage> viewModelCommunication)
		{
			ConfirmChanges = new Command(() => viewModelCommunication.Send(new ConfirmChanges()));
			RejectChanges  = new Command(() => viewModelCommunication.Send(new RejectChanges ()));
		}

		public ICommand ConfirmChanges { get; }
		public ICommand RejectChanges { get; }
	}
}
