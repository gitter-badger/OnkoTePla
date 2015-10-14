using System;
using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
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
