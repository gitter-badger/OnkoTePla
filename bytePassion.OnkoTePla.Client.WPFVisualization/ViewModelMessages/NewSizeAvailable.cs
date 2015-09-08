using System.Windows;
using bytePassion.Lib.Communication.ViewModel.Messages;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
{
	public class NewSizeAvailable : ViewModelMessage
	{
		public NewSizeAvailable(Size newSize)
		{
			NewSize = newSize;
		}

		public Size NewSize { get; }
	}
}
