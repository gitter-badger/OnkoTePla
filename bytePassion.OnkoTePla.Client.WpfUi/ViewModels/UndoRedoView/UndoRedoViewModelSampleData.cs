using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView
{
	public class UndoRedoViewModelSampleData : IUndoRedoViewModel
	{
		public ICommand Undo { get; } = null;
		public ICommand Redo { get; } = null;
		
		public void Dispose() { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}