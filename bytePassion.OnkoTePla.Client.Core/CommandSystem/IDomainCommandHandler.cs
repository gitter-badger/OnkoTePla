using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{

	public interface IDomainCommandHandler<in TCommand> : IMessageHandler<TCommand>
	{		
	}
}