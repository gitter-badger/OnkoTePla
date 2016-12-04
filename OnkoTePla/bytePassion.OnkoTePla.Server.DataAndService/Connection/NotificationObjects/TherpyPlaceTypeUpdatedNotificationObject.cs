using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects
{
	internal class TherpyPlaceTypeUpdatedNotificationObject : NotificationObject
	{
		public TherpyPlaceTypeUpdatedNotificationObject (TherapyPlaceType therapyPlaceType)
			: base(NetworkMessageType.TherapyPlaceTypeUpdatedNotification)
		{
			TherapyPlaceType = therapyPlaceType;
		}
		
		public TherapyPlaceType TherapyPlaceType { get; }
	}
}