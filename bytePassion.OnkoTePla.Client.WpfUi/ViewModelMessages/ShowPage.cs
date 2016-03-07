using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.Enums;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages
{
	internal class ShowPage : ViewModelMessage
	{		
		public ShowPage(MainPage page)
		{
			Page = page;
		}

		public MainPage Page { get; }
	}
}
