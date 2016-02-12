using bytePassion.Lib.Communication.MessageBus;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Base
{
	public interface IDomainEventHandler<in TEvent> : IMessageHandler<TEvent>
	{				
	}
}