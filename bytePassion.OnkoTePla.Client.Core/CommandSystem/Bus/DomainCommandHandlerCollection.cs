using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus
{

	public class DomainCommandHandlerCollection : IDomainCommandHandlerCollection
	{
		private readonly IDictionary<Type, object> commandHandlers;

		public DomainCommandHandlerCollection()
		{
			commandHandlers = new Dictionary<Type, object>();
		}

		public void Add<TCommand>(IDomainCommandHandler<TCommand> newCommandHandler) where TCommand : DomainCommand
		{
			if (commandHandlers.ContainsKey(typeof(TCommand)))
				throw new InvalidOperationException("There can only be one Commandhandler per Command");

			commandHandlers.Add(typeof(TCommand), newCommandHandler);
		}

		public IDomainCommandHandler<TCommand> GetDomainCommandHandlerFor<TCommand>() where TCommand : DomainCommand
		{
			if (!commandHandlers.ContainsKey(typeof(TCommand)))
				return null;

			return (IDomainCommandHandler<TCommand>) commandHandlers[typeof (TCommand)];
		}
	}

}