using System.Windows;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;

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
