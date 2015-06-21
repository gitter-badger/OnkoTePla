using bytePassion.OnkoTePla.Client.Core.CommandSystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus
{

	public interface ICommandBus {

		void RegisterCommandHandler<TCommand>(IDomainCommandHandler<TCommand> domainCommandHandler)
			where TCommand : DomainCommand;

		void Send<TCommand>(TCommand command) 
			where TCommand : DomainCommand;
	}

}