using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ThreadCollections
{
	internal interface INotificationThreadCollection : IDisposable
	{
		void StopThread(ConnectionSessionId sessionId);
		void AddThread(AddressIdentifier clientAddressIdentifier, ConnectionSessionId id);

		void SendNotification(NotificationObject notificationObject);
	}
}