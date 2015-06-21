using bytePassion.OnkoTePla.Client.Core.CommandSystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus
{

	public interface IDomainCommandHandlerCollection
	{
		void Add<TCommand>(IDomainCommandHandler<TCommand> newCommandHandler)
			where TCommand : DomainCommand;

		IDomainCommandHandler<TCommand> GetDomainCommandHandlerFor<TCommand>()
			where TCommand : DomainCommand;
	}

}