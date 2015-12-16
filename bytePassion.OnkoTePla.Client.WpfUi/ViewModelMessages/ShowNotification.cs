using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages
{
    public class ShowNotification : ViewModelMessage
	{
		public ShowNotification(string notificationMessage,
								int secondsToShow)
		{
			NotificationMessage = notificationMessage;
			SecondsToShow = secondsToShow;
		}

		public string NotificationMessage { get; }
		public int    SecondsToShow       { get; }
	}
}
