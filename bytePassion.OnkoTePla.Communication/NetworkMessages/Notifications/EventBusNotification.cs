using System;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.Notifications
{
	public class EventBusNotification : NetworkMessageBase
	{
		public EventBusNotification (DomainEvent newEvent, ConnectionSessionId sessionId) 
			: base(NetworkMessageType.EventBusNotification)
		{
			NewEvent = newEvent;
			SessionId = sessionId;
		}
		
		public DomainEvent         NewEvent  { get; }
		public ConnectionSessionId SessionId { get; }

		public override string AsString()
		{
			return $"{SessionId}|{DomainEventSerialization.Serialize(NewEvent)}";
		}

		public static EventBusNotification Parse (string s)
		{
			var index = s.IndexOf("|", StringComparison.Ordinal);

			var sessionId = new ConnectionSessionId(Guid.Parse(s.Substring(0, index)));
			var domainEvent = DomainEventSerialization.Deserialize(s.Substring(index + 1, s.Length - index - 1));

			return new EventBusNotification(domainEvent, sessionId);
		}
	}
}
