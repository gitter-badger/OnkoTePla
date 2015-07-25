using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{
	public interface ICommandBus
	{
		void RegisterCommandHandler<TDomainCommand>(IMessageHandler<TDomainCommand> commandHandler) 
			where TDomainCommand : DomainCommand;
		
		void DeregisterCommandHander<TDomainCommand>(IMessageHandler<TDomainCommand> commandHandler)
			where TDomainCommand : DomainCommand;

		void SendCommand<TDomainCommand>(TDomainCommand command) 
			where TDomainCommand : DomainCommand;
	}
}
