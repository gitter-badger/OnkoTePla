using bytePassion.Lib.Communication.ViewModel.Messages;
using System;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages
{
    public class HideNotification : ViewModelMessage
	{
		public HideNotification(Guid notificationId)
		{
			NotificationId = notificationId;
		}

		public Guid NotificationId { get; }
	}
}
