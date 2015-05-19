using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands.CommandBase;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{

	public interface ICommandBus {

		void RegisterCommandHandler<TCommand>(IDomainCommandHandler<TCommand> domainCommandHandler)
			where TCommand : DomainCommand;

		void Send<TCommand>(TCommand command) 
			where TCommand : DomainCommand;
	}

}