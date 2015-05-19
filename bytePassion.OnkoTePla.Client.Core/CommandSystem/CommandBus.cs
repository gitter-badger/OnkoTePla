using bytePassion.OnkoTePla.Client.Core.CommandSystem.CommandBase;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{

	public class CommandBus : ICommandBus
	{
		private readonly IDomainCommandHandlerCollection commandHandlerCollection;

		public CommandBus()
		{
			commandHandlerCollection = new DomainCommandHandlerCollection();
		}


		public void RegisterCommandHandler<TCommand>(IDomainCommandHandler<TCommand> domainCommandHandler)
			where TCommand : DomainCommand
		{
			commandHandlerCollection.Add(domainCommandHandler);			
		}


		public void Publish<TCommand>(TCommand command) where TCommand : DomainCommand
		{
			var commandHandler = commandHandlerCollection.GetDomainCommandHandlerFor<TCommand>();

			if (commandHandler != null)
				commandHandler.Execute(command);
		}
	}
}
