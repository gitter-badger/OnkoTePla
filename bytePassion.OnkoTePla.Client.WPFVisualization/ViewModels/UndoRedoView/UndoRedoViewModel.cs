using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfUtils.Commands;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView
{
	public class UndoRedoViewModel : IUndoRedoViewModel
	{
		private readonly IViewModelCommunication viewModelCommunication;

		public UndoRedoViewModel(IViewModelCommunication viewModelCommunication)
		{
			this.viewModelCommunication = viewModelCommunication;
			Undo = new Command(DoUndo);
			Redo = new Command(DoRedo);
		}

		private void DoRedo()
		{
			// TODO
		}

		private void DoUndo()
		{
			// TODO
		}

		public ICommand Undo { get; }
		public ICommand Redo { get; }
	}

}
