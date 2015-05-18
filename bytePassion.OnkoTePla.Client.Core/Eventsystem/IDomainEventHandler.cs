using System;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{
	public interface IDomainEventHandler<in TEvent>
	{
		void Handle(TEvent domainEvent);
		Type HandledEventType { get; }
	}
}