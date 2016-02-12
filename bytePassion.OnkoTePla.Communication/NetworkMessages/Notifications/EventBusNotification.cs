using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.Notifications
{
	public class EventBusNotification : NetworkMessageBase
	{
		public EventBusNotification (DomainEvent newEvent) 
			: base(NetworkMessageType.EventBusNotification)
		{
			NewEvent = newEvent;
		}

		public DomainEvent NewEvent { get; }

		public override string AsString()
		{
			return DomainEventSerialization.Serialize(NewEvent);
		}

		public static EventBusNotification Parse (string s)
		{
			return new EventBusNotification(DomainEventSerialization.Deserialize(s));
		}
	}
}
