using bytePassion.Lib.Communication.MessageBus;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem
{

	public interface IDomainCommandHandler<in TCommand> : IMessageHandler<TCommand>
	{		
	}
}