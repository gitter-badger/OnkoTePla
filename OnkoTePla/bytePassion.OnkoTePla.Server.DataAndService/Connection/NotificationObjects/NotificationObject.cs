using bytePassion.OnkoTePla.Communication.NetworkMessages;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects
{
	internal abstract class NotificationObject
	{
		protected NotificationObject(NetworkMessageType notificationType)
		{
			NotificationType = notificationType;
		}

		public NetworkMessageType NotificationType { get; }
	}
}
