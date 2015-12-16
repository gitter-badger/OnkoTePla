using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView
{
    public class ChangeConfirmationViewModel : ViewModel, 
                                               IChangeConfirmationViewModel
	{
		public ChangeConfirmationViewModel(IViewModelCommunication viewModelCommunication)
		{
			ConfirmChanges = new Command(() => viewModelCommunication.Send(new ConfirmChanges()));
			RejectChanges  = new Command(() => viewModelCommunication.Send(new RejectChanges ()));
		}

		public ICommand ConfirmChanges { get; }
		public ICommand RejectChanges  { get; }

        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
	}
}
