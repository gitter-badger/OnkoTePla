using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class TryToAddNewEventsRequest : NetworkMessageBase
	{
		public TryToAddNewEventsRequest(ConnectionSessionId sessionId, Guid userId, IReadOnlyList<DomainEvent> newEvents) 
			: base(NetworkMessageType.TryToAddNewEventsRequest)
		{
			SessionId = sessionId;
			UserId = userId;
			NewEvents = newEvents;			
		}

		public ConnectionSessionId        SessionId { get; }
		public Guid                       UserId    { get; }
		public IReadOnlyList<DomainEvent> NewEvents { get; }

		public override string AsString()
		{
			var sb = new StringBuilder();

			sb.Append(SessionId); sb.Append(';');
			sb.Append(UserId);    sb.Append(';');

			foreach (var domainEvent in NewEvents)
			{
				sb.Append(DomainEventSerialization.Serialize(domainEvent));
				sb.Append('#');
			}

			if (NewEvents.Count > 0)
				sb.Remove(sb.Length - 1, 1);

			return sb.ToString();
		}

		public static TryToAddNewEventsRequest Parse(string s)
		{
			var parts = s.Split(';');

			var sessionId = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId    =                         Guid.Parse(parts[1]);						
			var newEvents = parts[2].Split('#')
									.Select(DomainEventSerialization.Deserialize)
									.ToList();
			
			return new TryToAddNewEventsRequest(sessionId, userId, newEvents);
		}
	}
}
