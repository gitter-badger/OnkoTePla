using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects
{
	internal class EventNotificationObject : NotificationObject
	{
		public EventNotificationObject(DomainEvent domainEvent) 
			: base(NetworkMessageType.EventBusNotification)
		{
			DomainEvent = domainEvent;
		}

		public DomainEvent DomainEvent { get; }
	}
}