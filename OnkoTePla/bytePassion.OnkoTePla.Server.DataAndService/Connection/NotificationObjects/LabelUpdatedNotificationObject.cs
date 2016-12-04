using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects
{
	internal class LabelUpdatedNotificationObject : NotificationObject
	{
		public LabelUpdatedNotificationObject(Label label) 
			: base(NetworkMessageType.LabelUpdatedNotification)
		{
			Label = label;
		}

		public Label Label { get; }
	}
}