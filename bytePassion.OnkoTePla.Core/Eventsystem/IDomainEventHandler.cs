using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{
	public interface IDomainEventHandler<in TEvent> : IMessageHandler<TEvent>
	{				
	}
}