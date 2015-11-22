using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.Enums;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
{
	public class ShowPage : ViewModelMessage
	{		
		public ShowPage(MainPage page)
		{
			Page = page;
		}

		public MainPage Page { get; }
	}
}
