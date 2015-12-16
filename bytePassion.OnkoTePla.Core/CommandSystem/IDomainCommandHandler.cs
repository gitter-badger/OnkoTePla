using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.OnkoTePla.Core.CommandSystem
{

    public interface IDomainCommandHandler<in TCommand> : IMessageHandler<TCommand>
	{		
	}
}