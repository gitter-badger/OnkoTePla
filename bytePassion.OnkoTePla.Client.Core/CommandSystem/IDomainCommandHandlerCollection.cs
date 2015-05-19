using bytePassion.OnkoTePla.Client.Core.CommandSystem.CommandBase;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{

	public interface IDomainCommandHandlerCollection
	{
		void Add<TCommand>(IDomainCommandHandler<TCommand> newCommandHandler)
			where TCommand : DomainCommand;

		IDomainCommandHandler<TCommand> GetDomainCommandHandlerFor<TCommand>()
			where TCommand : DomainCommand;
	}

}