using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView
{
	public class UndoRedoViewModelSampleData : IUndoRedoViewModel
	{
		public ICommand Undo { get; } = null;
		public ICommand Redo { get; } = null;
	}
}