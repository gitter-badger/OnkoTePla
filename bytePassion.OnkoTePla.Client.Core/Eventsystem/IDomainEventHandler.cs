

using bytePassion.Lib.Messaging;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{
	public interface IDomainEventHandler<in TEvent> : IMessageHandler<TEvent>
	{				
	}
}