using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
{
	public class ShowNotification : ViewModelMessage
	{
		public ShowNotification(string notificationMessage)
		{
			NotificationMessage = notificationMessage;
		}

		public string NotificationMessage { get; }
	}
}
