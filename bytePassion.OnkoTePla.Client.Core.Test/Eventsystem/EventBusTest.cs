using System;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Contracts.Appointments;
using Xunit;

namespace bytePassion.OnkoTePla.Client.Core.Test.Eventsystem
{
	public class EventBusTest
	{

		private class TestSingleEventHandler : IDomainEventHandler<AppointmentAdded>
		{

			public TestSingleEventHandler()
			{
				HandledEvent = false;
			}

			public void Handle(AppointmentAdded domainEvent)
			{
				HandledEvent = true;
			}

			public bool HandledEvent { private set; get; }
		}
		
		[Fact]
		public void SingleSubscribscriptionTest()
		{
			IEventBus eventBus = new EventBus();
			var testEventHandler = new TestSingleEventHandler();

			eventBus.Subscribe(testEventHandler);
			eventBus.Publish(new AppointmentAdded(new Guid(), -1, new Guid(), new Appointment(null, null, null, new DateTime(), new DateTime())));

			Assert.True(testEventHandler.HandledEvent);
		}
		
		
		private class TestDoubleEventHandler : IDomainEventHandler<AppointmentAdded>,
											   IDomainEventHandler<AppointmentRemoved>
		{
			public TestDoubleEventHandler()
			{
				HandleAddedEvent = false;
				HandleRemovedEvent = false;
			}

			public void Handle(AppointmentAdded domainEvent)
			{
				HandleAddedEvent = true;
			}

			public void Handle(AppointmentRemoved domainEvent)
			{
				HandleRemovedEvent = true;
			}

			public bool HandleAddedEvent { private set; get; }
			public bool HandleRemovedEvent { private set; get; }
		}

		[Fact]
		public void DoubleSubscriptionTest()
		{
			IEventBus eventBus = new EventBus();
			var testEventHandler = new TestDoubleEventHandler();

			eventBus.Subscribe<AppointmentAdded>(testEventHandler);
			eventBus.Subscribe<AppointmentRemoved>(testEventHandler);

			Assert.False(testEventHandler.HandleAddedEvent);
			Assert.False(testEventHandler.HandleRemovedEvent);

			eventBus.Publish(new AppointmentAdded  (new Guid(), -1, new Guid(), new Appointment(null, null, null, new DateTime(), new DateTime())));

			Assert.True(testEventHandler.HandleAddedEvent);
			Assert.False(testEventHandler.HandleRemovedEvent);

			eventBus.Publish(new AppointmentRemoved(new Guid(), -1, new Guid(), new Appointment(null, null, null, new DateTime(), new DateTime())));

			Assert.True(testEventHandler.HandleAddedEvent);
			Assert.True(testEventHandler.HandleRemovedEvent);
		}
	}
}
