using System;
using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages
{
	internal class HideNotification : ViewModelMessage
	{
		public HideNotification(Guid notificationId)
		{
			NotificationId = notificationId;
		}

		public Guid NotificationId { get; }
	}
}
