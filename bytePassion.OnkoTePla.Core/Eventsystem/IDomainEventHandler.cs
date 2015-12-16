using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.OnkoTePla.Core.Eventsystem
{
    public interface IDomainEventHandler<in TEvent> : IMessageHandler<TEvent>
	{				
	}
}