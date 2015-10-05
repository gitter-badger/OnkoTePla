using System.Windows;
using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
{
	public class UIInstance : ViewModelMessage
	{
		public UIInstance(UIElement instance)
		{
			Instance = instance;
		}

		public UIElement Instance { get; }
	}
}
