using bytePassion.Lib.Communication.MessageBus;

namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{
	public class CommandBus : ICommandBus
	{
		private readonly IMessageBus<DomainCommand> commandMessageBus;

		public CommandBus(IMessageBus<DomainCommand> commandMessageBus)
		{
			this.commandMessageBus = commandMessageBus;
		}

		public void RegisterCommandHandler<TDomainCommand>(IMessageHandler<TDomainCommand> commandHandler) 
			where TDomainCommand : DomainCommand
		{
			commandMessageBus.RegisterMessageHandler(commandHandler);
		}

		public void DeregisterCommandHander<TDomainCommand>(IMessageHandler<TDomainCommand> commandHandler) 
			where TDomainCommand : DomainCommand
		{
			commandMessageBus.DeregisterMessageHander(commandHandler);
		}

		public void SendCommand<TDomainCommand>(TDomainCommand command) 
			where TDomainCommand : DomainCommand
		{
			commandMessageBus.Send(command);
		}
	}
}
