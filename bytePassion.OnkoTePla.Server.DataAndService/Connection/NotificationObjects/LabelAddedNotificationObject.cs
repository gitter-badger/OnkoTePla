using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects
{
	internal class LabelAddedNotificationObject : NotificationObject
	{
		public LabelAddedNotificationObject (Label label)
			: base(NetworkMessageType.LabelAddedNotification)
		{
			Label = label;			
		}

		public Label Label { get; }
	}
}