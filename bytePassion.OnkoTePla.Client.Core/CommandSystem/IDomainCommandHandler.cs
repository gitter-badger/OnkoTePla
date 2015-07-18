using bytePassion.Lib.Messaging;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{

	public interface IDomainCommandHandler<in TCommand> : IMessageHandler<TCommand>
	{		
	}
}