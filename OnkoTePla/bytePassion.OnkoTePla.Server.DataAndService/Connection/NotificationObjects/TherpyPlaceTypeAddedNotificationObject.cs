using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects
{
	internal class TherpyPlaceTypeAddedNotificationObject : NotificationObject
	{
		public TherpyPlaceTypeAddedNotificationObject(TherapyPlaceType therapyPlaceType)
			: base(NetworkMessageType.TherapyPlaceTypeAddedNotification)
		{
			TherapyPlaceType = therapyPlaceType;			
		}

		public TherapyPlaceType TherapyPlaceType { get; }
	}
}