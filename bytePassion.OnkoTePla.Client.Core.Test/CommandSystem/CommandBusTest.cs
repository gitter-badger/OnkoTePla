using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands;
using bytePassion.OnkoTePla.Contracts.Appointments;
using Xunit;

namespace bytePassion.OnkoTePla.Client.Core.Test.CommandSystem
{
	public class CommandBusTest
	{
		private class TestCommandHandler : IDomainCommandHandler<AddAppointment>
		{
			public TestCommandHandler()
			{
				CommandExecuted = false;
			}

			public void Execute(AddAppointment command)
			{
				CommandExecuted = true;
			}

			public bool CommandExecuted { private set; get; }
		}

		[Fact]
		public void CommandRegistrationAndExecutionTest()
		{
			ICommandBus commandBus = new CommandBus();
			var testCommandHandler = new TestCommandHandler();

			Assert.False(testCommandHandler.CommandExecuted);

			commandBus.RegisterCommandHandler(testCommandHandler);
			commandBus.Send(new AddAppointment(new Guid(), -1, new Appointment(null, null, null, new Date(1,2,3), new Time(1,2), new Time(1,2))));

			Assert.True(testCommandHandler.CommandExecuted);
		}
	}
}
