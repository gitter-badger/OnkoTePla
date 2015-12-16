using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Domain.Commands;
using bytePassion.OnkoTePla.Core.Eventsystem;
using System;
using Xunit;


namespace bytePassion.OnkoTePla.Core.Test.CommandSystem
{
    public class CommandBusTest
	{		

		private class TestCommandHandler : IDomainCommandHandler<AddAppointment>
		{
			public TestCommandHandler()
			{
				CommandExecuted = false;
			}

			public void Process(AddAppointment command)
			{
				CommandExecuted = true;
			}

			public bool CommandExecuted { private set; get; }
		}	

		[Fact]
		public void CommandRegistrationAndExecutionTest()
		{
			IHandlerCollection<DomainCommand> commandHandlerCollection = new SingleHandlerCollection<DomainCommand>();
			IMessageBus<DomainCommand> commandBus = new LocalMessageBus<DomainCommand>(commandHandlerCollection);
			var testCommandHandler = new TestCommandHandler();

			Assert.False(testCommandHandler.CommandExecuted);

			commandBus.RegisterMessageHandler(testCommandHandler);
			commandBus.Send(new AddAppointment(new AggregateIdentifier(Date.Dummy, new Guid()), 0, new Guid(), ActionTag.RegularAction, new Guid(), null, Time.Dummy, Time.Dummy, new Guid()));

			Assert.True(testCommandHandler.CommandExecuted);
		}
	}
}
