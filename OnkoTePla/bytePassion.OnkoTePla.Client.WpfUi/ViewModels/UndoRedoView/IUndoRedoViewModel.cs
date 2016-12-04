using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.UndoRedoView
{
    public interface IUndoRedoViewModel : IViewModel
	{
		ICommand Undo { get; }
		ICommand Redo { get; }
	}
}