using bytePassion.OnkoTePla.Client.Core.CommandSystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus
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


		public void Send<TCommand>(TCommand command) where TCommand : DomainCommand
		{
			var commandHandler = commandHandlerCollection.GetDomainCommandHandlerFor<TCommand>();

			if (commandHandler != null)
				commandHandler.Execute(command);
		}
	}
}
