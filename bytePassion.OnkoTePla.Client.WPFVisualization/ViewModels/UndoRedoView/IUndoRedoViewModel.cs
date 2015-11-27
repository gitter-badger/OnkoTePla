using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView
{
	public interface IUndoRedoViewModel : IViewModel
	{
		ICommand Undo { get; }
		ICommand Redo { get; }
	}
}