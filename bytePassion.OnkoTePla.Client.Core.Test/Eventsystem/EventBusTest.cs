using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Contracts.Appointments;
using Xunit;

namespace bytePassion.OnkoTePla.Client.Core.Test.Eventsystem
{
	public class EventBusTest
	{

		private class TestEventHandler : IDomainEventHandler<AppointmentAdded>
		{

			public TestEventHandler()
			{
				EventHandled = false;
			}

			public void Handle(AppointmentAdded domainEvent)
			{
				EventHandled = true;
			}

			public bool EventHandled { private set; get; }
		}

		[Fact]
		public void SubscribscriptionTest()
		{
			IEventBus eventBus = new EventBus();

			var testEventHandler = new TestEventHandler();

			eventBus.Subscribe(testEventHandler);

			eventBus.Publish(new AppointmentAdded(new Guid(), -1, new Guid(), new Appointment(null, null, null, new DateTime(), new DateTime())));

			Assert.True(testEventHandler.EventHandled);
		}
	}
}
